using Micro.Domain.Abstractions.Events;

namespace Micro.Domain.Abstractions.BuildingBlocks;

public interface IAggregate
{
    IEnumerable<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}