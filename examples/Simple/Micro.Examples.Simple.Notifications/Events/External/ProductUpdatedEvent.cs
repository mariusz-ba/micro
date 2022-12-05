using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Notifications.Events.External;

[Message("products-product-updated-event", "notifications-product-updated-event")]
internal record ProductUpdatedEvent(Guid ProductId, string Name, decimal Price) : IMessage;