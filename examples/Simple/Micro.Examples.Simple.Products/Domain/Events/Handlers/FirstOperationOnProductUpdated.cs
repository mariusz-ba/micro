using Micro.BackgroundJobs.Abstractions;
using Micro.CQRS.Abstractions.Commands;
using Micro.Domain.Abstractions.Events;
using Micro.Examples.Simple.Products.Commands;

namespace Micro.Examples.Simple.Products.Domain.Events.Handlers;

internal sealed class FirstOperationOnProductUpdated : IDomainEventHandler<ProductUpdatedDomainEvent>
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<FirstOperationOnProductUpdated> _logger;

    public FirstOperationOnProductUpdated(IBackgroundJobClient backgroundJobClient, ILogger<FirstOperationOnProductUpdated> logger)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
    }

    public async Task HandleAsync(ProductUpdatedDomainEvent domainEvent)
    {
        _logger.LogInformation("Product updated: Name - {Name}, Price - {Price}",
            domainEvent.Name, domainEvent.Price);
        await _backgroundJobClient.EnqueueAsync<ICommandDispatcher>(d =>
            d.SendAsync(new TrackDomainEventCommand(domainEvent)));
    }
}