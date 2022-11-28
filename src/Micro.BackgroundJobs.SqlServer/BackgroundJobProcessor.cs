using Micro.BackgroundJobs.SqlServer.Persistence;
using Micro.BackgroundJobs.SqlServer.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Micro.BackgroundJobs.SqlServer;

internal sealed class BackgroundJobProcessor<TContext> : BackgroundService where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BackgroundJobsOptions _backgroundJobsOptions;
    private readonly IBackgroundJobDataSerializer _serializer;
    private readonly ILogger<BackgroundJobProcessor<TContext>> _logger;
    private readonly Activity _activity;

    public BackgroundJobProcessor(
        IServiceProvider serviceProvider,
        BackgroundJobsOptions backgroundJobsOptions,
        IBackgroundJobDataSerializer serializer,
        ILogger<BackgroundJobProcessor<TContext>> logger)
    {
        _serviceProvider = serviceProvider;
        _backgroundJobsOptions = backgroundJobsOptions;
        _serializer = serializer;
        _logger = logger;
        _activity = new Activity("BackgroundJobProcessor");
    }

    public override void Dispose()
    {
        _activity.Dispose();
        base.Dispose();
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _activity.Start();
        _logger.LogInformation("Starting background job processor. Queue = {Queue}.", _backgroundJobsOptions.Queue);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping background job processor. Queue = {Queue}.", _backgroundJobsOptions.Queue);
        _activity.Stop();
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            var jobsCount = await ProcessJobsAsync(stoppingToken);
            if (jobsCount == 0)
            {
                await Task.Delay(_backgroundJobsOptions.FetchJobsDelay, stoppingToken);
            }
        }
    }

    private async Task<int> ProcessJobsAsync(CancellationToken stoppingToken)
    {
        var jobs = await GetEnqueuedJobs(stoppingToken);
        
        foreach (var job in jobs)
        {
            var activity = new Activity("ProcessJob");
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                if (job.Data is null)
                {
                    job.State = BackgroundJobState.Success;
                    job.ProcessedAt = DateTime.UtcNow;
                    job.InvisibleUntil = null;
                    continue;
                }
                
                var jobHandlerDescriptor = _serializer.Deserialize(job.Data);
                jobHandlerDescriptor.Activity?.Attach(activity);

                activity.Start();
                
                await ProcessJobAsync(jobHandlerDescriptor);

                job.State = BackgroundJobState.Success;
                job.ProcessedAt = DateTime.UtcNow;
                job.InvisibleUntil = null;
            }
            catch (BackgroundJobDataSerializerException exception)
            {
                job.State = BackgroundJobState.Failure;
                job.ProcessedAt = DateTime.UtcNow;
                job.InvisibleUntil = null;
                job.ErrorMessage = exception.Message;
                
                _logger.LogError(exception, exception.Message);
            }
            catch (Exception exception)
            {
                var currentAttempt = job.RetryAttempt;
                
                if (job.RetryAttempt >= job.RetryMaxCount)
                {
                    job.State = BackgroundJobState.Failure;
                    job.ProcessedAt = DateTime.UtcNow;
                    job.InvisibleUntil = null;
                    job.ErrorMessage = exception.Message;
                }
                else
                {
                    job.RetryAttempt++;
                    job.ProcessedAt = DateTime.UtcNow;
                    job.InvisibleUntil = DateTime.UtcNow.AddSeconds(Math.Pow(_backgroundJobsOptions.RetrySecondsBase, job.RetryAttempt));
                    job.ErrorMessage = exception.Message;
                }
                
                _logger.LogError(exception, exception.Message);
                _logger.LogError(
                    "Failed to process background job. Attempt {RetryAttempt} out of {RetryMaxCount}.",
                    currentAttempt, job.RetryMaxCount);
            }
            finally
            {
                stopwatch.Stop();
                activity.Dispose();

                job.ProcessingDuration = stopwatch.ElapsedMilliseconds;

                await using var scope = _serviceProvider.CreateAsyncScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
                var entry = dbContext.Attach(job);
                entry.Property(x => x.State).IsModified = true;
                entry.Property(x => x.ProcessedAt).IsModified = true;
                entry.Property(x => x.ErrorMessage).IsModified = true;
                entry.Property(x => x.RetryAttempt).IsModified = true;
                entry.Property(x => x.InvisibleUntil).IsModified = true;
                entry.Property(x => x.ProcessingDuration).IsModified = true;
                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }

        return jobs.Count;
    }

    private async Task ProcessJobAsync(BackgroundJobHandlerDescriptor handlerDescriptor)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var jobHandler = scope.ServiceProvider.GetService(handlerDescriptor.HandlerType);
        if (jobHandler is null)
        {
            throw new BackgroundJobDataSerializerException("Could not resolve target handler type from DI.");
        }

        var result = handlerDescriptor.HandlerMethod.Invoke(jobHandler, handlerDescriptor.HandlerMethodParameters);
        if (result is Task task)
        {
            await task;
        }
    }
    
    private async Task<List<BackgroundJob>> GetEnqueuedJobs(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        
        return await dbContext.Set<BackgroundJob>()
            .FromSqlInterpolated($@"
WITH [Jobs] AS (
    SELECT TOP ({_backgroundJobsOptions.FetchJobsBatchSize}) *
    FROM [Micro].[BackgroundJobs] WITH (ROWLOCK, READPAST)
    WHERE
        [Queue] = {_backgroundJobsOptions.Queue} AND
        [State] = 'Enqueued' AND
        ([InvisibleUntil] IS NULL OR [InvisibleUntil] < GETUTCDATE())
    ORDER BY [CreatedAt]
)
UPDATE [Jobs]
SET
    [ServerId] = {Environment.MachineName},
    [InvisibleUntil] = {DateTime.UtcNow.Add(_backgroundJobsOptions.FetchJobsTimeout)}
OUTPUT inserted.*
").ToListAsync(stoppingToken);
    }
}