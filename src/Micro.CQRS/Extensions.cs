using Micro.CQRS.Abstractions.Commands;
using Micro.CQRS.Abstractions.Queries;
using Micro.CQRS.Commands;
using Micro.CQRS.Queries;
using Micro.Common.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.CQRS;

public static class Extensions
{
    public static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableToAny(typeof(ICommandHandler<,>), typeof(IQueryHandler<,>))
                .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}