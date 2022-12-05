namespace Micro.Messaging.Abstractions;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(MessageEnvelope<TMessage> message, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage;
}