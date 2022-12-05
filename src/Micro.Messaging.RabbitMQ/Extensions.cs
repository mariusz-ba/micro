using Micro.Messaging.Abstractions;
using Micro.Messaging.RabbitMQ.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Messaging.RabbitMQ;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddSingleton<IMessageBrokerConventions, RabbitMqConventions>();
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddSingleton<IMessageSubscriber, RabbitMqMessageSubscriber>();

        return services;
    }
}