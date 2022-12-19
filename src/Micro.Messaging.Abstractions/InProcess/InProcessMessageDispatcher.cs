using Microsoft.Extensions.DependencyInjection;

namespace Micro.Messaging.Abstractions.InProcess;

internal sealed class InProcessMessageDispatcher : IMessageDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public InProcessMessageDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<TMessage>(MessageEnvelope<TMessage> envelope, CancellationToken cancellationToken = default)
        where TMessage : class, IMessage
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>();
        await handler.HandleAsync(envelope.Message, envelope.Context.Metadata);
    }
}