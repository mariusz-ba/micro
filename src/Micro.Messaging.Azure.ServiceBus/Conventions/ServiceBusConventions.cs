using Humanizer;
using Micro.Common.App;
using Micro.Messaging.Abstractions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Micro.Messaging.Azure.ServiceBus.Conventions;

internal sealed class ServiceBusConventions : IMessageBrokerConventions
{
    private readonly IOptions<AppOptions> _appOptions;

    public ServiceBusConventions(IOptions<AppOptions> appOptions)
    {
        _appOptions = appOptions;
    }

    public string GetTopicName(Type messageType)
    {
        var attribute = messageType.GetCustomAttribute<MessageAttribute>();
        return attribute is null
            ? $"{_appOptions.Value.Name.Kebaberize()}-{messageType.Name.Kebaberize()}"
            : $"{attribute.ProducerService.Kebaberize()}-{messageType.Name.Kebaberize()}";
    }

    public string? GetSubscriptionName(Type messageType)
    {
        var attribute = messageType.GetCustomAttribute<MessageAttribute>();
        return attribute?.IsExternalMessage is true
            ? $"{_appOptions.Value.Name.Kebaberize()}-{messageType.Name.Kebaberize()}"
            : null;
    }
}