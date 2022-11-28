namespace Micro.BackgroundJobs.SqlServer.Persistence;

public sealed class BackgroundJob
{
    internal Guid Id { get; init; }
    internal BackgroundJobState State { get; set; }
    internal BackgroundJobData? Data { get; init; }
    internal string Queue { get; init; } = string.Empty;
    internal int RetryAttempt { get; set; }
    internal int RetryMaxCount { get; init; }
    internal string? ErrorMessage { get; set; }
    internal DateTime CreatedAt { get; init; }
    internal DateTime? ProcessedAt { get; set; }
    internal DateTime? InvisibleUntil { get; set; }
    internal long? ProcessingDuration { get; set; }
    internal string? ServerId { get; set; }
}