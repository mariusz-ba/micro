using Micro.Common.Decorators;
using Micro.Messaging.Abstractions.InProcess;
using Micro.Messaging.Abstractions.Registration;
using Micro.Messaging.Abstractions.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Messaging.Abstractions;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IMessageHandler<>))
                .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services
            .AddSingleton<IMessageSerializer, JsonMessageSerializer>()
            .AddSingleton<IMessagePublisher, InProcessMessagePublisher>()
            .AddSingleton<IMessageDispatcher, InProcessMessageDispatcher>()
            .AddSingleton<ISubscribedMessagesProvider, SubscribedMessagesProvider>()
            .AddHostedService<MessageListenerService>();
    }
}