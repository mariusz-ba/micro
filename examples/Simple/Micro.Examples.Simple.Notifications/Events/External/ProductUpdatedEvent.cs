using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Notifications.Events.External;

[Message("Products.ProductUpdatedEvent", "Notifications.ProductUpdatedEvent")]
internal record ProductUpdatedEvent(Guid ProductId, string Name, decimal Price) : IMessage;