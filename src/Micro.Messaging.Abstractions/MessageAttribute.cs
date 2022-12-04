namespace Micro.Messaging.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public class MessageAttribute : Attribute
{
    public string Topic { get; }
    public string? Subscription { get; }

    public MessageAttribute(string topic, string? subscription = null)
    {
        Topic = topic;
        Subscription = subscription;
    }
}
