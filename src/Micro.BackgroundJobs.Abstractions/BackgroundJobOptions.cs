namespace Micro.BackgroundJobs.Abstractions;

public class BackgroundJobOptions
{
    public string Queue { get; set; } = "Default";
    public int RetryCount { get; set; } = 5;
    public DateTime? VisibleFrom { get; set; }
}