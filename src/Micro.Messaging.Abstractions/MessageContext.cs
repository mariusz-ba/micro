namespace Micro.Messaging.Abstractions;

public class MessageContext
{
    public MessageMetadata Metadata { get; }
    public MessageActivity Activity { get; }

    public MessageContext(MessageMetadata metadata, MessageActivity activity)
    {
        Metadata = metadata;
        Activity = activity;
    }

    public static MessageContext Create()
        => new(new MessageMetadata(Guid.NewGuid().ToString()), MessageActivity.Create());
}