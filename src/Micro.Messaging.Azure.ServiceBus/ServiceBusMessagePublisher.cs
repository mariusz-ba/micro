using Azure.Messaging.ServiceBus;
using Micro.Messaging.Abstractions.Serialization;
using Micro.Messaging.Abstractions;
using Micro.Messaging.Azure.ServiceBus.Conventions;
using System.Collections.Concurrent;

namespace Micro.Messaging.Azure.ServiceBus;

internal sealed class ServiceBusMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly ConcurrentDictionary<string, ServiceBusSender> _senders = new();

    private readonly ServiceBusClient _serviceBusClient;
    private readonly IMessageSerializer _messageSerializer;
    private readonly IMessageBrokerConventions _messageBrokerConventions;

    public ServiceBusMessagePublisher(
        ServiceBusClient serviceBusClient,
        IMessageSerializer messageSerializer,
        IMessageBrokerConventions messageBrokerConventions)
    {
        _serviceBusClient = serviceBusClient;
        _messageSerializer = messageSerializer;
        _messageBrokerConventions = messageBrokerConventions;
    }

    public async Task PublishAsync<TMessage>(MessageEnvelope<TMessage> message, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        var sender = _senders.GetOrAdd(
            _messageBrokerConventions.GetTopicName(typeof(TMessage)),
            topic => _serviceBusClient.CreateSender(topic));

        var messageSerialized = _messageSerializer.SerializeBytes(message);
        var messageToSend = new ServiceBusMessage(messageSerialized)
        {
            MessageId = message.Context.MessageId,
            CorrelationId = message.Context.TraceId
        };

        await sender.SendMessageAsync(messageToSend, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var sender in _senders.Values)
        {
            await sender.DisposeAsync();
        }
    }
}