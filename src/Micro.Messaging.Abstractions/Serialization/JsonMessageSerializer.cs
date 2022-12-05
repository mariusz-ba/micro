using System.Text.Json.Serialization;
using System.Text.Json;

namespace Micro.Messaging.Abstractions.Serialization;

internal sealed class JsonMessageSerializer : IMessageSerializer
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public byte[] SerializeBytes<TMessage>(TMessage message)
        => JsonSerializer.SerializeToUtf8Bytes(message, JsonSerializerOptions);

    public TMessage? DeserializeBytes<TMessage>(byte[] message)
        => JsonSerializer.Deserialize<TMessage>(message, JsonSerializerOptions);
}