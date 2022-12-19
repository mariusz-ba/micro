namespace Micro.Messaging.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public class MessageAttribute : Attribute
{
    public string ProducerService { get; }
    public bool IsExternalMessage { get; }

    public MessageAttribute(string producerService, bool isExternalMessage = true)
    {
        ProducerService = producerService;
        IsExternalMessage = isExternalMessage;
    }
}
