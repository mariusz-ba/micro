using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Products.Events;

internal record ProductUpdatedEvent(Guid ProductId, string Name, decimal Price) : IMessage;