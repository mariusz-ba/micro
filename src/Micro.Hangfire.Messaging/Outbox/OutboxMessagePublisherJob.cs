using Hangfire;
using Micro.Messaging.Abstractions;

namespace Micro.Hangfire.Messaging.Outbox;

internal sealed class OutboxMessagePublisherJob
{
    private readonly IMessageBroker _messageBroker;

    public OutboxMessagePublisherJob(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    [Queue("message-outbox")]
    [MessageJobDisplayName("Message: {0}")]
    public Task PublishAsync<TMessage>(MessageEnvelope<TMessage> envelope) where TMessage : class, IMessage
        => _messageBroker.PublishAsync(envelope);
}