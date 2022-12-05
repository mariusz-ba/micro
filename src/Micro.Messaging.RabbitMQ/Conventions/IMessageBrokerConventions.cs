namespace Micro.Messaging.RabbitMQ.Conventions;

internal interface IMessageBrokerConventions
{
    string GetExchangeName(Type messageType);
    string? GetQueueName(Type messageType);
}