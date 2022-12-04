using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Notifications.Events.External;

[Message("Products.ProductCreatedEvent", "Notifications.ProductCreatedEvent")]
internal record ProductCreatedEvent(Guid ProductId, string Name, decimal Price) : IMessage;