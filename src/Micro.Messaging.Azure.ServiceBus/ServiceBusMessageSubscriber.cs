using Azure.Messaging.ServiceBus;
using Micro.Messaging.Abstractions.Serialization;
using Micro.Messaging.Abstractions;
using Micro.Messaging.Azure.ServiceBus.Conventions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Micro.Messaging.Azure.ServiceBus;

internal sealed class ServiceBusMessageSubscriber : IMessageSubscriber, IAsyncDisposable
{
    private readonly ConcurrentDictionary<string, ServiceBusProcessor> _processors = new();

    private readonly ServiceBusClient _serviceBusClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageSerializer _messageSerializer;
    private readonly IMessageBrokerConventions _messageBrokerConventions;
    private readonly ILogger<ServiceBusMessageSubscriber> _logger;

    public ServiceBusMessageSubscriber(
        ServiceBusClient serviceBusClient,
        IServiceProvider serviceProvider,
        IMessageSerializer messageSerializer,
        IMessageBrokerConventions messageBrokerConventions,
        ILogger<ServiceBusMessageSubscriber> logger)
    {
        _serviceBusClient = serviceBusClient;
        _serviceProvider = serviceProvider;
        _messageSerializer = messageSerializer;
        _messageBrokerConventions = messageBrokerConventions;
        _logger = logger;
    }

    public IMessageSubscriber Subscribe<TMessage>(
        Func<MessageEnvelope<TMessage>, IServiceProvider, CancellationToken, Task> handler)
        where TMessage : class, IMessage
    {
        var topic = _messageBrokerConventions.GetTopicName(typeof(TMessage));
        var subscription = _messageBrokerConventions.GetSubscriptionName(typeof(TMessage));
        if (subscription is null)
        {
            throw new InvalidOperationException($"Subscription is not defined on message with type '{typeof(TMessage).Name}'.");
        }

        var processor = _processors.GetOrAdd(topic + subscription, _ => _serviceBusClient.CreateProcessor(topic, subscription));

        processor.ProcessMessageAsync += async args =>
        {
            var messageDeserialized = _messageSerializer.DeserializeBytes<MessageEnvelope<TMessage>>(args.Message.Body.ToArray());
            if (messageDeserialized is not null)
            {
                using var activity = new Activity($"Message {topic}/{subscription}");
                messageDeserialized.Context.Activity.AttachTo(activity);
                activity.Start();

                try
                {
                    await handler.Invoke(messageDeserialized, _serviceProvider, args.CancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, exception.Message);
                }
            }

            await args.CompleteMessageAsync(args.Message, args.CancellationToken);
        };

        processor.ProcessErrorAsync += args =>
        { 
            _logger.LogError(args.Exception, args.Exception.Message);
            return Task.CompletedTask;
        };

        if (processor.IsProcessing is false)
        {
            processor.StartProcessingAsync();
        }

        return this;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var processor in _processors.Values)
        {
            await processor.StopProcessingAsync();
            await processor.DisposeAsync();
        }
    }
}