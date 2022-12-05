using Micro.Messaging.Abstractions.Serialization;
using Micro.Messaging.Abstractions;
using Micro.Messaging.RabbitMQ.Connections;
using Micro.Messaging.RabbitMQ.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Diagnostics;

namespace Micro.Messaging.RabbitMQ;

internal sealed class RabbitMqMessageSubscriber : IMessageSubscriber
{
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageSerializer _messageSerializer;
    private readonly IMessageBrokerConventions _messageBrokerConventions;
    private readonly ILogger<RabbitMqMessageSubscriber> _logger;

    public RabbitMqMessageSubscriber(
        IChannelFactory channelFactory,
        IServiceProvider serviceProvider,
        IMessageSerializer messageSerializer,
        IMessageBrokerConventions messageBrokerConventions,
        ILogger<RabbitMqMessageSubscriber> logger)
    {
        _messageSerializer = messageSerializer;
        _messageBrokerConventions = messageBrokerConventions;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _channel = channelFactory.Create();
    }
    
    public IMessageSubscriber Subscribe<TMessage>(Func<MessageEnvelope<TMessage>, IServiceProvider, CancellationToken, Task> handler)
        where TMessage : class, IMessage
    {
        var exchange = _messageBrokerConventions.GetExchangeName(typeof(TMessage));
        var queue = _messageBrokerConventions.GetQueueName(typeof(TMessage));
        if (queue is null)
        {
            throw new InvalidOperationException($"Queue is not defined on message with type '{typeof(TMessage).Name}'.");
        }
        
        _channel.ExchangeDeclare(exchange, ExchangeType.Fanout, durable: true);
        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue, exchange, routingKey: string.Empty);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (_, args) =>
        {
            var messageDeserialized = _messageSerializer.DeserializeBytes<MessageEnvelope<TMessage>>(args.Body.ToArray());
            if (messageDeserialized is not null)
            {
                using var activity = new Activity($"Subscribe {exchange}/{queue}");
                messageDeserialized.Context.Attach(activity);
                activity.Start();

                try
                {
                    await using var scope = _serviceProvider.CreateAsyncScope();
                    await handler.Invoke(messageDeserialized, scope.ServiceProvider, CancellationToken.None);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                }
            }
            
            _channel.BasicAck(args.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue, autoAck: false, consumer);

        return this;
    }
}