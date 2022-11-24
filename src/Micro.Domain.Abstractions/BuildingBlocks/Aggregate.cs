using Micro.Domain.Abstractions.Events;

namespace Micro.Domain.Abstractions.BuildingBlocks;

public abstract class Aggregate<TEntityId> : Entity<TEntityId>, IAggregate
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IEnumerable<IDomainEvent> DomainEvents => _domainEvents;
    
    protected Aggregate(TEntityId id) : base(id)
    {
    }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}