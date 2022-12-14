namespace Micro.Messaging.Abstractions;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(MessageEnvelope<TMessage> envelope, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage;
}