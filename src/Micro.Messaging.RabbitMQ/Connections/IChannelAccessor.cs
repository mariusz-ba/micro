using RabbitMQ.Client;

namespace Micro.Messaging.RabbitMQ.Connections;

internal interface IChannelAccessor
{
    IModel? Channel { get; set; }
}