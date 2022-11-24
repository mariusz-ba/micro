using Micro.Domain.Abstractions.BuildingBlocks;
using Micro.Domain.Abstractions.Events;
using Microsoft.EntityFrameworkCore;

namespace Micro.Persistence.SqlServer.Events;

internal sealed class DbContextDomainEventProvider<TContext> : IDomainEventProvider where TContext : DbContext
{
    private readonly TContext _dbContext;

    public DbContextDomainEventProvider(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<IDomainEvent> GetDomainEvents()
    {
        var domainEvents = new List<IDomainEvent>();

        var aggregates = _dbContext.ChangeTracker
            .Entries<IAggregate>()
            .Select(e => e.Entity)
            .ToList();

        foreach (var aggregate in aggregates)
        {
            domainEvents.AddRange(aggregate.DomainEvents);
            aggregate.ClearDomainEvents();
        }

        return domainEvents;
    }
}