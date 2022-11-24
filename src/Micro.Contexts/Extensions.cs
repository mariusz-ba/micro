using Micro.Contexts.Abstractions;
using Micro.Contexts.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Contexts;

public static class Extensions
{
    public static IServiceCollection AddContexts(this IServiceCollection services)
    {
        services.AddSingleton<IExecutionContextProvider, ExecutionContextProvider>();
        services.AddScoped<ExecutionContextUserMiddleware>();
        
        return services;
    }

    public static IApplicationBuilder UseContexts(this IApplicationBuilder app)
        => app.UseMiddleware<ExecutionContextUserMiddleware>();
}