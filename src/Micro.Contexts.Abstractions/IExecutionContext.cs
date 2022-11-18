namespace Micro.Contexts.Abstractions;

public interface IExecutionContext
{
    public string TraceId { get; }
    public string? UserId { get; }
}