using Micro.Messaging.Abstractions;

namespace Micro.Messaging.RabbitMQ;

internal sealed class RabbitMqMessageSubscriber : IMessageSubscriber
{
    public IMessageSubscriber Subscribe<TMessage>(Func<MessageEnvelope<TMessage>, IServiceProvider, CancellationToken, Task> handler)
        where TMessage : class, IMessage
    {
        throw new NotImplementedException();
    }
}