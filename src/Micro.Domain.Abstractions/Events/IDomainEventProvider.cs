namespace Micro.Domain.Abstractions.Events;

public interface IDomainEventProvider
{
    IEnumerable<IDomainEvent> GetDomainEvents();
}