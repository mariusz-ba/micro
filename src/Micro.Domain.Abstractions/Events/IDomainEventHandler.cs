namespace Micro.Domain.Abstractions.Events;

public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : class, IDomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent);
}