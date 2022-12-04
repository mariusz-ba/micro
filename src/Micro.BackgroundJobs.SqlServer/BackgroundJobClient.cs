using Micro.BackgroundJobs.Abstractions;
using Micro.BackgroundJobs.SqlServer.Persistence;
using Micro.BackgroundJobs.SqlServer.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Micro.BackgroundJobs.SqlServer;

internal sealed class BackgroundJobClient<TContext> : IBackgroundJobClient where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly IBackgroundJobDataSerializer _serializer;
    
    public BackgroundJobClient(TContext dbContext, IBackgroundJobDataSerializer serializer)
    {
        _dbContext = dbContext;
        _serializer = serializer;
    }
    
    public void Enqueue<THandler>(Expression<Func<THandler, Task>> expression, BackgroundJobOptions? options = null)
    {
        var backgroundJobData = _serializer.Serialize(expression);
        var backgroundJob = new BackgroundJob
        {
            Id = Guid.NewGuid(),
            State = BackgroundJobState.Enqueued,
            Queue = options?.Queue ?? "Default",
            Data = backgroundJobData,
            CreatedAt = DateTime.UtcNow,
            InvisibleUntil = options?.VisibleFrom,
            RetryMaxCount = options?.RetryCount ?? 5
        };

        _dbContext.Set<BackgroundJob>().Add(backgroundJob);
    }

    public async Task EnqueueAsync<THandler>(Expression<Func<THandler, Task>> expression, BackgroundJobOptions? options = null)
    {
        Enqueue(expression, options);
        await _dbContext.SaveChangesAsync();
    }
}