using Micro.Domain.Abstractions.Events;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class SecondOperationOnProductUpdated : IDomainEventHandler<ProductUpdatedDomainEvent>
{
    private readonly ILogger<SecondOperationOnProductUpdated> _logger;

    public SecondOperationOnProductUpdated(ILogger<SecondOperationOnProductUpdated> logger)
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