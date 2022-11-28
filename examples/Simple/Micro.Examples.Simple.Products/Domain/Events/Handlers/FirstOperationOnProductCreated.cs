using Micro.BackgroundJobs.Abstractions;
using Micro.CQRS.Abstractions.Commands;
using Micro.Domain.Abstractions.Events;
using Micro.Examples.Simple.Products.Commands;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class FirstOperationOnProductCreated : IDomainEventHandler<ProductCreatedDomainEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<FirstOperationOnProductCreated> _logger;

    public FirstOperationOnProductCreated(IBackgroundJobClient backgroundJobClient, ILogger<FirstOperationOnProductCreated> logger)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
    }

    public Task HandleAsync(ProductCreatedDomainEvent domainEvent)
    {
        _logger.LogInformation("Product created: Name - {Name}, Price - {Price}", domainEvent.Name, domainEvent.Price);
        _backgroundJobClient.Enqueue<ICommandDispatcher>(d =>
            d.SendAsync(new TrackDomainEventCommand(domainEvent)));
        return Task.CompletedTask;
    }
}