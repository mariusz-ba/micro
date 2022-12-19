using Micro.Messaging.Abstractions.Serialization;
using Micro.Messaging.Abstractions;
using Micro.Messaging.RabbitMQ.Connections;
using Micro.Messaging.RabbitMQ.Conventions;
using RabbitMQ.Client;

namespace Micro.Messaging.RabbitMQ;

internal sealed class RabbitMqMessageBroker : IMessageBroker
{
    private readonly IModel _channel;
    private readonly IMessageSerializer _messageSerializer;
    private readonly IMessageBrokerConventions _messageBrokerConventions;

    public RabbitMqMessageBroker(
        IChannelFactory channelFactory,
        IMessageSerializer messageSerializer,
        IMessageBrokerConventions messageBrokerConventions)
    {
        _messageSerializer = messageSerializer;
        _messageBrokerConventions = messageBrokerConventions;
        _channel = channelFactory.Create();
    }
    
    public Task PublishAsync<TMessage>(MessageEnvelope<TMessage> envelope, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        var exchange = _messageBrokerConventions.GetExchangeName(typeof(TMessage));
        
        var messageSerialized = _messageSerializer.SerializeBytes(envelope);

        var properties = _channel.CreateBasicProperties();
        properties.MessageId = envelope.Context.Metadata.MessageId;
        properties.CorrelationId = envelope.Context.Activity.TraceId; 
        _channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true);
        _channel.BasicPublish(exchange, routingKey: string.Empty, mandatory: true, properties, messageSerialized);

        return Task.CompletedTask;
    }
}