using Micro.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Hangfire.Messaging.Inbox;

public static class Extensions
{
    public static IServiceCollection AddHangfireMessagingInbox(this IServiceCollection services)
        => services
            .AddSingleton<IMessageDispatcher, InboxMessageDispatcher>()
            .AddSingleton<InboxMessageDispatcherJob>();
}