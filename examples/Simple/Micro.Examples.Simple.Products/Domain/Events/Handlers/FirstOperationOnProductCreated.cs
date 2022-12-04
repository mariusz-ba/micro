using Micro.BackgroundJobs.Abstractions;
using Micro.Domain.Abstractions.Events;
using Micro.Examples.Simple.Products.Events;
using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class FirstOperationOnProductCreated : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public FirstOperationOnProductCreated(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public Task HandleAsync(ProductCreatedDomainEvent domainEvent)
    {
        _backgroundJobClient.Enqueue<IMessagePublisher>(p => p.PublishAsync(
                MessageEnvelope<ProductCreatedEvent>.Create(new ProductCreatedEvent(
                    domainEvent.ProductId,
                    domainEvent.Name,
                    domainEvent.Price)), CancellationToken.None),
            new BackgroundJobOptions { Queue = "MessageOutbox" });

        return Task.CompletedTask;
    }
}