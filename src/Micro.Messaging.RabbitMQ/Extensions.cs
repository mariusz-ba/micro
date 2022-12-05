using Micro.Messaging.Abstractions;
using Micro.Messaging.RabbitMQ.Connections;
using Micro.Messaging.RabbitMQ.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Micro.Messaging.RabbitMQ;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>();
        var rabbitMqConnection = new ConnectionFactory { Uri = new Uri(rabbitMqOptions.ConnectionString) }.CreateConnection();

        services.AddSingleton(rabbitMqConnection);

        services.AddSingleton<IChannelFactory, ChannelFactory>();
        services.AddSingleton<IChannelAccessor, ChannelAccessor>();

        services.AddSingleton<IMessageBrokerConventions, RabbitMqConventions>();
        services.AddSingleton<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddSingleton<IMessageSubscriber, RabbitMqMessageSubscriber>();

        return services;
    }
}