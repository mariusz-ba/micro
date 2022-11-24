namespace Micro.Contexts.Abstractions;

public interface IExecutionContext
{
    string? ActivityId { get; }
    string? TraceId { get; }
    string? UserId { get; }
}