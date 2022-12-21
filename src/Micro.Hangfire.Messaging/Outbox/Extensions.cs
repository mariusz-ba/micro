using Micro.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Hangfire.Messaging.Outbox;

public static class Extensions
{
    public static IServiceCollection AddHangfireMessagingOutbox(this IServiceCollection services)
        => services
            .AddScoped<IMessagePublisher, OutboxMessagePublisher>()
            .AddSingleton<OutboxMessagePublisherJob>();
}