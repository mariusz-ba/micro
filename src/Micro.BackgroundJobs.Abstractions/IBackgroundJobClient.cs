using System.Linq.Expressions;

namespace Micro.BackgroundJobs.Abstractions;

public interface IBackgroundJobClient
{
    void Enqueue<THandler>(Expression<Func<THandler, Task>> expression, BackgroundJobOptions? options = null);
    Task EnqueueAsync<THandler>(Expression<Func<THandler, Task>> expression, BackgroundJobOptions? options = null);
}