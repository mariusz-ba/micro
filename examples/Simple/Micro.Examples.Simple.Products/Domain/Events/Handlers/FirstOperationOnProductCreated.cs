using Micro.Domain.Abstractions.Events;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class FirstOperationOnProductCreated : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly ILogger<FirstOperationOnProductCreated> _logger;

    public FirstOperationOnProductCreated(ILogger<FirstOperationOnProductCreated> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(ProductCreatedDomainEvent domainEvent)
    {
        _logger.LogInformation("Product created: Name - {Name}, Price - {Price}", domainEvent.Name, domainEvent.Price);
        return Task.CompletedTask;
    }
}