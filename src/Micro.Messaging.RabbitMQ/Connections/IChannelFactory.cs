using RabbitMQ.Client;

namespace Micro.Messaging.RabbitMQ.Connections;

internal interface IChannelFactory
{
    IModel Create();
}