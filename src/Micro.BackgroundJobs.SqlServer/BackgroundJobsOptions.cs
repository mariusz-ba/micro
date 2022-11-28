namespace Micro.BackgroundJobs.SqlServer;

public class BackgroundJobsOptions
{
    public string Queue { get; set; } = "Default";
    public TimeSpan FetchJobsTimeout { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan FetchJobsDelay { get; set; } = TimeSpan.FromSeconds(5);
    public int FetchJobsBatchSize { get; set; } = 10;
    public int RetrySecondsBase { get; set; } = 2;
    public TimeSpan CleanerInterval { get; set; } = TimeSpan.FromHours(1);
    public TimeSpan KeepSuccessJobsTimeout { get; set; } = TimeSpan.FromDays(1);
    public TimeSpan KeepFailureJobsTimeout { get; set; } = TimeSpan.FromDays(7);
}