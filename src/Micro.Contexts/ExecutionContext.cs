using Micro.Contexts.Abstractions;
using System.Diagnostics;

namespace Micro.Contexts;

internal sealed class ExecutionContext : IExecutionContext
{
    public string? ActivityId { get; }
    public string? TraceId { get; }
    public string? UserId { get; }

    public ExecutionContext()
    {
        ActivityId = Activity.Current?.Id;
        TraceId = Activity.Current?.TraceId.ToString();
        UserId = Activity.Current?.GetTagItem("UserId")?.ToString();
    }

    public ExecutionContext(string? activityId, string? traceId, string? userId)
    {
        ActivityId = activityId;
        TraceId = traceId;
        UserId = userId;
    }
}