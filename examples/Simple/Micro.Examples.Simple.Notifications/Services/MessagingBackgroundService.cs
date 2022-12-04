using Micro.Examples.Simple.Notifications.Events.External;
using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Notifications.Services;

internal sealed class MessagingBackgroundService : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;

    public MessagingBackgroundService(IMessageSubscriber messageSubscriber)
    {
        _messageSubscriber = messageSubscriber;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageSubscriber.Subscribe<ProductCreatedEvent>((envelope, sp, _) =>
        {
            var logger = sp.GetRequiredService<ILogger<MessagingBackgroundService>>();

            logger.LogInformation("Product created: {ProductId}, {Name}, {Price}",
                envelope.Message.ProductId, envelope.Message.Name, envelope.Message.Price);

            return Task.CompletedTask;
        });
        
        _messageSubscriber.Subscribe<ProductUpdatedEvent>((envelope, sp, _) =>
        {
            var logger = sp.GetRequiredService<ILogger<MessagingBackgroundService>>();

            logger.LogInformation("Product updated: {ProductId}, {Name}, {Price}",
                envelope.Message.ProductId, envelope.Message.Name, envelope.Message.Price);

            return Task.CompletedTask;
        });
        
        return Task.CompletedTask;
    }
}