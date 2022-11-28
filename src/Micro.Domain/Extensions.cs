using Micro.Common.Decorators;
using Micro.Domain.Abstractions.Events;
using Micro.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Micro.Domain;

public static class Extensions
{
    public static IServiceCollection AddDomainEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>))
                .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}