namespace Micro.BackgroundJobs.EntityFrameworkCore.Persistence;

internal sealed class BackgroundJobData
{
    public string HandlerType { get; init; } = string.Empty;
    public string[] HandlerGenericArguments { get; init; } = Array.Empty<string>();
    public string HandlerMethod { get; init; } = string.Empty;
    public string[] HandlerMethodGenericArguments { get; init; } = Array.Empty<string>();
    public object?[] HandlerMethodParameters { get; init; } = Array.Empty<object?>();
    public BackgroundJobActivity? Activity { get; init; }
}