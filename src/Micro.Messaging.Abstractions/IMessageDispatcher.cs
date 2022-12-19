namespace Micro.Messaging.Abstractions;

public interface IMessageDispatcher
{
    Task SendAsync<TMessage>(MessageEnvelope<TMessage> envelope, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage;
}