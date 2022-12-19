using Azure.Messaging.ServiceBus.Administration;
using Azure.Messaging.ServiceBus;
using Micro.Messaging.Abstractions;
using Micro.Messaging.Azure.ServiceBus.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Messaging.Azure.ServiceBus;

public static class Extensions
{
    public static IServiceCollection AddAzureServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceBusOptions = configuration.GetSection(ServiceBusOptions.SectionName).Get<ServiceBusOptions>();

        services.AddSingleton(new ServiceBusClient(serviceBusOptions.ConnectionString));
        services.AddSingleton(new ServiceBusAdministrationClient(serviceBusOptions.ConnectionString));

        services.AddSingleton<IMessageBrokerConventions, ServiceBusConventions>();
        services.AddSingleton<IMessageBroker, ServiceBusMessageBroker>();
        services.AddSingleton<IMessageSubscriber, ServiceBusMessageSubscriber>();

        services.AddHostedService<ServiceBusInitializer>();

        return services;
    }
}