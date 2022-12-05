namespace Micro.Messaging.Abstractions.Serialization;

public interface IMessageSerializer
{
    byte[] SerializeBytes<TMessage>(TMessage message);
    TMessage? DeserializeBytes<TMessage>(byte[] message);
}