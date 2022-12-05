using Micro.BackgroundJobs.Abstractions;
using Micro.Domain.Abstractions.Events;
using Micro.Examples.Simple.Products.Events;
using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class FirstOperationOnProductUpdated : IDomainEventHandler<ProductUpdatedDomainEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public FirstOperationOnProductUpdated(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public Task HandleAsync(ProductUpdatedDomainEvent domainEvent)
    {
        _backgroundJobClient.Enqueue<IMessagePublisher>(p => p.PublishAsync(
                MessageEnvelope<ProductUpdatedEvent>.Create(new ProductUpdatedEvent(
                    domainEvent.ProductId,
                    domainEvent.Name,
                    domainEvent.Price)), CancellationToken.None),
            new BackgroundJobOptions { Queue = "MessageOutbox" });

        return Task.CompletedTask;
    }
}