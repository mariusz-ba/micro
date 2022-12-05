using Micro.Messaging.Abstractions.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Messaging.Abstractions;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IMessageSerializer, JsonMessageSerializer>();

        return services;
    }
}