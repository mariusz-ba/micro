namespace Micro.Messaging.Abstractions;

public interface IMessageSubscriber
{
    IMessageSubscriber Subscribe<TMessage>(Func<MessageEnvelope<TMessage>, IServiceProvider, CancellationToken, Task> handler)
        where TMessage : class, IMessage;
}