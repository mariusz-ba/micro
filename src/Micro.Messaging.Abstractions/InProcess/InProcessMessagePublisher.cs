namespace Micro.Messaging.Abstractions.InProcess;

internal sealed class InProcessMessagePublisher : IMessagePublisher
{
    private readonly IMessageBroker _messageBroker;

    public InProcessMessagePublisher(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    public Task PublishAsync<TMessage>(MessageEnvelope<TMessage> envelope, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
        => _messageBroker.PublishAsync(envelope, cancellationToken);
}