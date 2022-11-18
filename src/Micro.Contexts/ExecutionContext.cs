using Micro.Contexts.Abstractions;

namespace Micro.Contexts;

internal sealed record ExecutionContext : IExecutionContext
{
    public string TraceId { get; set; } = Guid.NewGuid().ToString("N");
    public string? UserId { get; set; }
}