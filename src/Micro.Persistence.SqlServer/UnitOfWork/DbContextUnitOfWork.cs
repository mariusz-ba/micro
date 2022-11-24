using Micro.Domain.Abstractions.Events;
using Micro.Persistence.Abstractions.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Micro.Persistence.SqlServer.UnitOfWork;

internal sealed class DbContextUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly IDomainEventProvider _domainEventProvider;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public DbContextUnitOfWork(TContext dbContext, IDomainEventProvider domainEventProvider, IDomainEventDispatcher domainEventDispatcher)
    {
        _dbContext = dbContext;
        _domainEventProvider = domainEventProvider;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        await action.Invoke();
        await PublishDomainEvents();
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    private async Task PublishDomainEvents()
    {
        var domainEvents = _domainEventProvider.GetDomainEvents();
        foreach (var domainEvent in domainEvents)
        {
            await _domainEventDispatcher.PublishAsync(domainEvent);
        }
    }
}