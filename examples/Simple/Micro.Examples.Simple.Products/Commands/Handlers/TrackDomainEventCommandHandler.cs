using Micro.CQRS.Abstractions.Commands;
using System.Text.Json;

namespace Micro.Examples.Simple.Products.Commands.Handlers;

internal sealed class TrackDomainEventCommandHandler : ICommandHandler<TrackDomainEventCommand>
{
    private readonly ILogger<TrackDomainEventCommandHandler> _logger;

    public TrackDomainEventCommandHandler(ILogger<TrackDomainEventCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(TrackDomainEventCommand command)
    {
        var domainEvent = JsonSerializer.Serialize(command.DomainEvent, command.DomainEvent.GetType());
        _logger.LogInformation("[Tracker]: {DomainEvent}", domainEvent);
        return Task.CompletedTask;
    }
}