using Micro.Domain.Abstractions.Events;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class FirstOperationOnProductUpdated : IDomainEventHandler<ProductUpdatedDomainEvent>
{
    private readonly ILogger<FirstOperationOnProductUpdated> _logger;

    public FirstOperationOnProductUpdated(ILogger<FirstOperationOnProductUpdated> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(ProductUpdatedDomainEvent domainEvent)
    {
        _logger.LogInformation("Product updated: Name - {Name}, Price - {Price}",
            domainEvent.Name, domainEvent.Price);
        return Task.CompletedTask;
    }
}