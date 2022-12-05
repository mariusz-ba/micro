using Micro.Messaging.Abstractions;

namespace Micro.Messaging.RabbitMQ;

internal sealed class RabbitMqMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<TMessage>(MessageEnvelope<TMessage> message, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        throw new NotImplementedException();
    }
}