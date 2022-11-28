namespace Micro.BackgroundJobs.SqlServer.Persistence;

internal enum BackgroundJobState
{
    Enqueued,
    Success,
    Failure
}