using Micro.Contexts.Abstractions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Micro.Observability.ApplicationInsights.TelemetryInitializers;

internal sealed class ExecutionContextTelemetryInitializer : ITelemetryInitializer
{
    private readonly IExecutionContextProvider _executionContextProvider;

    public ExecutionContextTelemetryInitializer(IExecutionContextProvider executionContextProvider)
    {
        _executionContextProvider = executionContextProvider;
    }

    public void Initialize(ITelemetry telemetry)
    {
        var executionContext = _executionContextProvider.GetContext();
        if (executionContext.UserId is null) return;
        telemetry.Context.User.Id = executionContext.UserId;
        telemetry.Context.User.AuthenticatedUserId = executionContext.UserId;
    }
}