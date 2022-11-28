namespace Micro.Persistence.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
    Task ExecuteAsync(Func<Task> action);
}