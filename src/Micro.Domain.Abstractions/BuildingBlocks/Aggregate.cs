using Micro.Domain.Abstractions.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.Domain.Abstractions.BuildingBlocks;

public abstract class Aggregate<TEntityId> : IEntity<TEntityId>, IAggregate
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public abstract TEntityId Id { get; }

    [NotMapped]
    public IEnumerable<IDomainEvent> DomainEvents => _domainEvents;
    
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}