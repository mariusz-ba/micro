namespace Micro.BackgroundJobs.EntityFrameworkCore.Persistence;

internal enum BackgroundJobState
{
    Enqueued,
    Success,
    Failure
}