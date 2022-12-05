using Micro.Messaging.Abstractions.Serialization;
using Micro.Messaging.Abstractions;
using Micro.Messaging.RabbitMQ.Connections;
using Micro.Messaging.RabbitMQ.Conventions;
using RabbitMQ.Client;

namespace Micro.Messaging.RabbitMQ;

internal sealed class RabbitMqMessagePublisher : IMessagePublisher
{
    private readonly IModel _channel;
    private readonly IMessageSerializer _messageSerializer;
    private readonly IMessageBrokerConventions _messageBrokerConventions;

    public RabbitMqMessagePublisher(
        IChannelFactory channelFactory,
        IMessageSerializer messageSerializer,
        IMessageBrokerConventions messageBrokerConventions)
    {
        _messageSerializer = messageSerializer;
        _messageBrokerConventions = messageBrokerConventions;
        _channel = channelFactory.Create();
    }
    
    public Task PublishAsync<TMessage>(MessageEnvelope<TMessage> message, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        var exchange = _messageBrokerConventions.GetExchangeName(typeof(TMessage));
        
        var messageSerialized = _messageSerializer.SerializeBytes(message);

        var properties = _channel.CreateBasicProperties();
        properties.MessageId = message.Context.MessageId;
        properties.CorrelationId = message.Context.TraceId; 
        _channel.ExchangeDeclare(exchange, type: "fanout", durable: true);
        _channel.BasicPublish(exchange, routingKey: string.Empty, mandatory: true, properties, messageSerialized);

        return Task.CompletedTask;
    }
}