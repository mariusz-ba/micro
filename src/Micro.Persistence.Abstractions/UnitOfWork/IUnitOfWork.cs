namespace Micro.Persistence.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(Func<Task> action);
}