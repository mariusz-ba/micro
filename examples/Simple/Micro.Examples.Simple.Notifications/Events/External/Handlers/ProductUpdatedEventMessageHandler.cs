using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Notifications.Events.External.Handlers;

internal sealed class ProductUpdatedEventMessageHandler : IMessageHandler<ProductUpdatedEvent>
{
    private readonly ILogger<ProductUpdatedEventMessageHandler> _logger;

    public ProductUpdatedEventMessageHandler(ILogger<ProductUpdatedEventMessageHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(ProductUpdatedEvent message, MessageMetadata metadata)
    {
        _logger.LogInformation("Product updated: {ProductId}, {Name}, {Price}",
            message.ProductId, message.Name, message.Price);
        
        return Task.CompletedTask;
    }
}