using System.Linq.Expressions;

namespace Micro.BackgroundJobs.Abstractions;

public interface IBackgroundJobClient
{
    Task EnqueueAsync<THandler>(Expression<Func<THandler, Task>> expression, BackgroundJobOptions? options = null);
}