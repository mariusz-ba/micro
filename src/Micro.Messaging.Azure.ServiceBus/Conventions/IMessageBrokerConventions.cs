namespace Micro.Messaging.Azure.ServiceBus.Conventions;

internal interface IMessageBrokerConventions
{
    string GetTopicName(Type messageType);
    string? GetSubscriptionName(Type messageType);
}