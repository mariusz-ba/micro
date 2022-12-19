using Micro.Domain.Abstractions.Events;
using Micro.Persistence.Abstractions.Exceptions;
using Micro.Persistence.Abstractions.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace Micro.Persistence.SqlServer.UnitOfWork;

internal sealed class DbContextUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TContext _dbContext;

    public DbContextUnitOfWork(IServiceProvider serviceProvider, TContext dbContext)
    {
        _serviceProvider = serviceProvider;
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync()
    {
        using var transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            TransactionScopeAsyncFlowOption.Enabled);

        await PublishDomainEventsAsync();
        var result = await SaveChangesAndHandleExceptionsAsync();
        transactionScope.Complete();
        return result;
    }

    public async Task<int> SaveChangesAsync(Func<Task> action)
    {
        using var transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            TransactionScopeAsyncFlowOption.Enabled);

        await action.Invoke();
        await PublishDomainEventsAsync();
        var result = await SaveChangesAndHandleExceptionsAsync();
        transactionScope.Complete();
        return result;
    }

    private async Task<int> SaveChangesAndHandleExceptionsAsync()
    {
        try
        {
            return await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new EntityConcurrentUpdateException(exception);
        }
        catch (DbUpdateException exception)
        {
            if (exception.InnerException is SqlException { Number: 2601 })
            {
                throw new EntityDuplicateException(exception);
            }

            throw;
        }
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEventProvider = _serviceProvider.GetService<IDomainEventProvider>();
        if (domainEventProvider is null)
        {
            return;
        }
        
        var domainEventDispatcher = _serviceProvider.GetService<IDomainEventDispatcher>();
        if (domainEventDispatcher is null)
        {
            return;
        }

        var domainEvents = domainEventProvider.GetDomainEvents();

        while (domainEvents.Count > 0)
        {
            foreach (var domainEvent in domainEvents)
            {
                await domainEventDispatcher.PublishAsync(domainEvent);
            }

            domainEvents = domainEventProvider.GetDomainEvents();
        }
    }
}