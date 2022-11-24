namespace Micro.Persistence.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}