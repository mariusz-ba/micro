using Hangfire;
using Micro.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Hangfire.Messaging.Inbox;

internal sealed class InboxMessageDispatcher : IMessageDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public InboxMessageDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<TMessage>(MessageEnvelope<TMessage> envelope, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var backgroundJobClient = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
        backgroundJobClient.Enqueue<InboxMessageDispatcherJob>(d => d.SendAsync(envelope));
    }
}