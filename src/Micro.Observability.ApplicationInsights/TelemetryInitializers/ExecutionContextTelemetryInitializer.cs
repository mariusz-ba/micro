using Micro.Contexts.Abstractions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Micro.Observability.ApplicationInsights.TelemetryInitializers;

internal sealed class ExecutionContextTelemetryInitializer : ITelemetryInitializer
{
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public ExecutionContextTelemetryInitializer(IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        var executionContext = _executionContextAccessor.Context;
        if (executionContext?.TraceId is not null)
        {
            telemetry.Context.Operation.Id = executionContext.TraceId;
        }

        if (executionContext?.UserId is not null)
        {
            telemetry.Context.User.Id = executionContext.UserId;
            telemetry.Context.User.AuthenticatedUserId = executionContext.UserId;
        }
    }
}