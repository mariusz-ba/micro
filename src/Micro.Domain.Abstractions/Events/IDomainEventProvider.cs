namespace Micro.Domain.Abstractions.Events;

public interface IDomainEventProvider
{
    IList<IDomainEvent> GetDomainEvents();
}