using Micro.CQRS.Abstractions.Commands;
using Micro.CQRS.Abstractions.Queries;
using Micro.CQRS.Commands;
using Micro.CQRS.Queries;
using Micro.Common.Decorators;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Micro.CQRS;

public static class Extensions
{
    public static IServiceCollection AddCQRS(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableToAny(typeof(ICommandHandler<,>), typeof(IQueryHandler<,>))
                .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}