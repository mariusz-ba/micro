using Micro.Contexts.Abstractions;
using Micro.Observability.ApplicationInsights.TelemetryInitializers;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Observability.ApplicationInsights;

public static class Extensions
{
    public static IServiceCollection AddObservability(this IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetry();
        services.AddSingleton<ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();

        if (services.Any(d => d.ServiceType == typeof(IExecutionContextProvider)))
        {
            services.AddSingleton<ITelemetryInitializer, ExecutionContextTelemetryInitializer>();
        }

        return services;
    }
}