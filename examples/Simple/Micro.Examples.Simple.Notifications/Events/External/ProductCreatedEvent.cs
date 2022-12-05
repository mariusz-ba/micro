using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Notifications.Events.External;

[Message("products-product-created-event", "notifications-product-created-event")]
internal record ProductCreatedEvent(Guid ProductId, string Name, decimal Price) : IMessage;