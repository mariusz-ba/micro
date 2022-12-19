using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Notifications.Events.External.Handlers;

internal sealed class ProductCreatedEventMessageHandler : IMessageHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventMessageHandler> _logger;

    public ProductCreatedEventMessageHandler(ILogger<ProductCreatedEventMessageHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(ProductCreatedEvent message, MessageMetadata metadata)
    {
        _logger.LogInformation("Product created: {ProductId}, {Name}, {Price}",
            message.ProductId, message.Name, message.Price);
        
        return Task.CompletedTask;
    }
}