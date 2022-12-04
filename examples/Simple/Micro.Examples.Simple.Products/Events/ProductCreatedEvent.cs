using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Products.Events;

internal record ProductCreatedEvent(Guid ProductId, string Name, decimal Price) : IMessage;