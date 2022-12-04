namespace Micro.Messaging.Abstractions;

public class MessageEnvelope<TMessage> where TMessage : class, IMessage
{
    public TMessage Message { get; }
    public MessageContext Context { get; }

    public MessageEnvelope(TMessage message, MessageContext context)
    {
        Message = message;
        Context = context;
    }

    public static MessageEnvelope<TMessage> Create(TMessage message) =>
        new(message, MessageContext.Create());
}