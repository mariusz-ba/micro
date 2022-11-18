namespace Micro.Domain.Abstractions.Events;

public interface IDomainEventDispatcher
{
    Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IDomainEvent;
}