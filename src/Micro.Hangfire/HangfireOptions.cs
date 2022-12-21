namespace Micro.Hangfire;

internal class HangfireOptions
{
    public const string SectionName = "Hangfire";
    public bool MigrateDatabase { get; set; }
    public HangfireStorageOptions? Storage { get; set; }
    public HangfireServerOptions[]? Servers { get; set; }
}

internal class HangfireStorageOptions
{
    public TimeSpan? QueuePollInterval { get; set; }
    public TimeSpan? CommandBatchMaxTimeout { get; set; }
    public TimeSpan? SlidingInvisibilityTimeout { get; set; }
}

internal class HangfireServerOptions
{
    public int? WorkerCount { get; set; }
    public string[]? Queues { get; set; }
}