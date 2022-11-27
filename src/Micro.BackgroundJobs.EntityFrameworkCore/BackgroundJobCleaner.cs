using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Micro.BackgroundJobs.EntityFrameworkCore;

internal sealed class BackgroundJobCleaner<TContext> : BackgroundService where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BackgroundJobsOptions _backgroundJobsOptions;
    private readonly ILogger<BackgroundJobCleaner<TContext>> _logger;
    private readonly Activity _activity;

    public BackgroundJobCleaner(
        IServiceProvider serviceProvider,
        BackgroundJobsOptions backgroundJobsOptions,
        ILogger<BackgroundJobCleaner<TContext>> logger)
    {
        _serviceProvider = serviceProvider;
        _backgroundJobsOptions = backgroundJobsOptions;
        _logger = logger;
        _activity = new Activity("BackgroundJobCleaner");
    }

    public override void Dispose()
    {
        _activity.Dispose();
        base.Dispose();
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _activity.Start();
        _logger.LogInformation("Starting background job cleaner. Queue = {Queue}.", _backgroundJobsOptions.Queue);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping background job cleaner. Queue = {Queue}.", _backgroundJobsOptions.Queue);
        _activity.Stop();
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            var jobsCount = await DeleteSuccessAndFailureJobsAsync(stoppingToken);
            
            _logger.LogInformation("Cleared {JobsCount} jobs. Queue = {Queue}.", jobsCount, _backgroundJobsOptions.Queue);
            
            await Task.Delay(_backgroundJobsOptions.CleanerInterval, stoppingToken);
        }
    }

    private async Task<int> DeleteSuccessAndFailureJobsAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        return await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM [Micro].[BackgroundJobs]
            WHERE
                ([Queue] = {_backgroundJobsOptions.Queue} AND [State] = 'Success' AND DATEDIFF(MILLISECOND, [ProcessedAt], GETUTCDATE()) >= {_backgroundJobsOptions.KeepSuccessJobsTimeout.TotalMilliseconds}) OR
                ([Queue] = {_backgroundJobsOptions.Queue} AND [State] = 'Failure' AND DATEDIFF(MILLISECOND, [ProcessedAt], GETUTCDATE()) >= {_backgroundJobsOptions.KeepFailureJobsTimeout.TotalMilliseconds});",
            stoppingToken);
    }
}