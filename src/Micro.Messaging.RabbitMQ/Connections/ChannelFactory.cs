using RabbitMQ.Client;

namespace Micro.Messaging.RabbitMQ.Connections;

internal sealed class ChannelFactory : IChannelFactory
{
    private readonly IConnection _connection;
    private readonly IChannelAccessor _channelAccessor;

    public ChannelFactory(IConnection connection, IChannelAccessor channelAccessor)
    {
        _connection = connection;
        _channelAccessor = channelAccessor;
    }

    public IModel Create() => _channelAccessor.Channel ??= _connection.CreateModel();
}