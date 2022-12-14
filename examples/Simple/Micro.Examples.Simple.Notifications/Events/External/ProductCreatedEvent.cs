using Micro.Messaging.Abstractions;

namespace Micro.Examples.Simple.Notifications.Events.External;

[Message("products")]
internal record ProductCreatedEvent(Guid ProductId, string Name, decimal Price) : IMessage;