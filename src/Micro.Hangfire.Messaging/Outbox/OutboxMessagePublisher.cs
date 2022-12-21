using Hangfire;
using Micro.Messaging.Abstractions;

namespace Micro.Hangfire.Messaging.Outbox;

internal sealed class OutboxMessagePublisher : IMessagePublisher
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public OutboxMessagePublisher(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public Task PublishAsync<TMessage>(MessageEnvelope<TMessage> envelope, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        _backgroundJobClient.Enqueue<OutboxMessagePublisherJob>(p => p.PublishAsync(envelope));
        
        return Task.CompletedTask;
    }
}