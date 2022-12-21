using Hangfire;
using Micro.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Micro.Hangfire.Messaging.Inbox;

internal sealed class InboxMessageDispatcherJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InboxMessageDispatcherJob> _logger;

    public InboxMessageDispatcherJob(IServiceProvider serviceProvider, ILogger<InboxMessageDispatcherJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    [Queue("message-inbox")]
    [MessageJobDisplayName("Message: {0}")]
    public async Task SendAsync<TMessage>(MessageEnvelope<TMessage> envelope) where TMessage : class, IMessage
    {
        using var activity = new Activity($"Message {envelope.Message.GetType().Name}");
        envelope.Context.Activity.AttachTo(activity);
        activity.Start();

        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>();
            await handler.HandleAsync(envelope.Message, envelope.Context.Metadata);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            throw;
        }
    }
}