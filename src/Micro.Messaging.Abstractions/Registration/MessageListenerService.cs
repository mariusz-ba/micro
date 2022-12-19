using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Micro.Messaging.Abstractions.Registration;

internal sealed class MessageListenerService : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly ISubscribedMessagesProvider _subscribedMessagesProvider;

    public MessageListenerService(
        IMessageSubscriber messageSubscriber,
        ISubscribedMessagesProvider subscribedMessagesProvider)
    {
        _messageSubscriber = messageSubscriber;
        _subscribedMessagesProvider = subscribedMessagesProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var messagesToSubscribeTo = _subscribedMessagesProvider.GetMessagesToSubscribeTo();
        
        var subscribeMethod = typeof(MessageListenerService).GetMethod(nameof(SubscribeMessage), BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var messageType in messagesToSubscribeTo)
        {
            subscribeMethod?.MakeGenericMethod(messageType).Invoke(this, Array.Empty<object>());
        }

        return Task.CompletedTask;
    }

    private void SubscribeMessage<TMessage>() where TMessage : class, IMessage
    {
        _messageSubscriber.Subscribe<TMessage>(async (envelope, sp, cancellationToken) =>
        {
            var messageDispatcher = sp.GetRequiredService<IMessageDispatcher>();
            await messageDispatcher.SendAsync(envelope, cancellationToken);
        });
    }
}