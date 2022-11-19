using Micro.Common.App;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;

namespace Micro.Observability.ApplicationInsights.TelemetryInitializers;

internal sealed class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
{
    private readonly IOptions<AppOptions> _appOptions;

    public CloudRoleNameTelemetryInitializer(IOptions<AppOptions> appOptions)
    {
        _appOptions = appOptions;
    }

    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = _appOptions.Value.Name;
    }
}