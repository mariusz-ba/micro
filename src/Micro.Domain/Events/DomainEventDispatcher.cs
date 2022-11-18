using Micro.Domain.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Domain.Events;

internal sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IDomainEvent
    {
        using var scope = _serviceProvider.CreateScope();
        var handlersType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = scope.ServiceProvider.GetServices(handlersType);

        foreach (var handler in handlers)
        {
            await (Task)handlersType
                .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))?
                .Invoke(handler, new object?[] { domainEvent })!;
        }
    }
}