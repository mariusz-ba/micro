using Micro.Domain.Abstractions.Events;
using Micro.Examples.Simple.Products.Events;
using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class FirstOperationOnProductCreated : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly IMessagePublisher _messagePublisher;

    public FirstOperationOnProductCreated(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    public Task HandleAsync(ProductCreatedDomainEvent domainEvent)
        => _messagePublisher.PublishAsync(
            MessageEnvelope<ProductCreatedEvent>.Create(new ProductCreatedEvent(
                domainEvent.ProductId,
                domainEvent.Name,
                domainEvent.Price)));
}