using Micro.CQRS.Abstractions.Commands;
using Micro.Domain.Abstractions.Events;

namespace Micro.Examples.Simple.Products.Commands;

public record TrackDomainEventCommand(IDomainEvent DomainEvent) : ICommand;