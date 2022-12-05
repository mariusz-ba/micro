using RabbitMQ.Client;

namespace Micro.Messaging.RabbitMQ.Connections;

internal sealed class ChannelAccessor : IChannelAccessor
{
    private static readonly ThreadLocal<ChannelHolder> Holder = new();

    public IModel? Channel
    {
        get => Holder.Value?.Channel;
        set
        {
            var holder = Holder.Value;
            if (holder is not null)
            {
                holder.Channel = null;
            }

            if (value is not null)
            {
                Holder.Value = new ChannelHolder { Channel = value };
            }
        }
    }

    private class ChannelHolder
    {
        public IModel? Channel;
    }
}