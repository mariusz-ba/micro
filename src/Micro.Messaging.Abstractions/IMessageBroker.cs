namespace Micro.Messaging.Abstractions;

public interface IMessageBroker
{
    Task PublishAsync<TMessage>(MessageEnvelope<TMessage> envelope, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage;
}