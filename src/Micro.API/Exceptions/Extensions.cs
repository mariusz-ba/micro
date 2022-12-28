using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.API.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddExceptionsHandling(this IServiceCollection services)
    {
        services.AddScoped<ExceptionsHandlerMiddleware>();
        services.AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();
        services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
        services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo<IExceptionToResponseMapper>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        return services;
    }

    public static IApplicationBuilder UseExceptionsHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsHandlerMiddleware>();
        
        return app;
    }
}