namespace Micro.Messaging.Abstractions.Serialization;

public interface IMessageSerializer
{
    string Serialize<TMessage>(TMessage message);
    TMessage? Deserialize<TMessage>(string message);
    byte[] SerializeBytes<TMessage>(TMessage message);
    TMessage? DeserializeBytes<TMessage>(byte[] message);
}