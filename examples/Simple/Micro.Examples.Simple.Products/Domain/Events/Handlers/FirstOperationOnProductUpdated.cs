using Micro.Domain.Abstractions.Events;
using Micro.Examples.Simple.Products.Events;
using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class FirstOperationOnProductUpdated : IDomainEventHandler<ProductUpdatedDomainEvent>
{
    private readonly IMessagePublisher _messagePublisher;

    public FirstOperationOnProductUpdated(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    public Task HandleAsync(ProductUpdatedDomainEvent domainEvent)
        => _messagePublisher.PublishAsync(
            MessageEnvelope<ProductUpdatedEvent>.Create(new ProductUpdatedEvent(
                domainEvent.ProductId,
                domainEvent.Name,
                domainEvent.Price)));
}