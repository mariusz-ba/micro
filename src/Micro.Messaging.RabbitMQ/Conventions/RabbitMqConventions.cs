using Humanizer;
using Micro.Common.App;
using Micro.Messaging.Abstractions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Micro.Messaging.RabbitMQ.Conventions;

internal sealed class RabbitMqConventions : IMessageBrokerConventions
{
    private readonly IOptions<AppOptions> _appOptions;

    public RabbitMqConventions(IOptions<AppOptions> appOptions)
    {
        _appOptions = appOptions;
    }

    public string GetExchangeName(Type messageType)
    {
        var attribute = messageType.GetCustomAttribute<MessageAttribute>();
        return attribute is null
            ? $"{_appOptions.Value.Name.Kebaberize()}-{messageType.Name.Kebaberize()}"
            : string.Join('-', attribute.Topic.Split('.').Select(part => part.Kebaberize()));
    }

    public string? GetQueueName(Type messageType)
    {
        var attribute = messageType.GetCustomAttribute<MessageAttribute>();
        return attribute?.Subscription is null
            ? null
            : string.Join('-', attribute.Subscription.Split('.').Select(part => part.Kebaberize()));
    }
}