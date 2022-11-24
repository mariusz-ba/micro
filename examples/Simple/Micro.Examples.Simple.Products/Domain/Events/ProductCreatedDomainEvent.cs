using Micro.Domain.Abstractions.Events;

namespace Micro.Examples.Simple.Products.Domain.Events;

public record ProductCreatedDomainEvent(Guid ProductId, string Name, decimal Price) : IDomainEvent;