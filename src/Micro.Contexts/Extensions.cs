using Micro.Contexts.Abstractions;
using Micro.Contexts.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Contexts;

public static class Extensions
{
    public static IServiceCollection AddContexts(this IServiceCollection services)
    {
        services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
        services.AddScoped<ExecutionContextMiddleware>();
        services.AddScoped<IdentityContextMiddleware>();
        
        return services;
    }

    public static IApplicationBuilder UseExecutionContext(this IApplicationBuilder app)
        => app.UseMiddleware<ExecutionContextMiddleware>();

    public static IApplicationBuilder UseIdentityContext(this IApplicationBuilder app)
        => app.UseMiddleware<IdentityContextMiddleware>();
}