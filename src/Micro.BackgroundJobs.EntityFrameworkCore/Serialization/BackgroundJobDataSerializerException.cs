namespace Micro.BackgroundJobs.EntityFrameworkCore.Serialization;

internal class BackgroundJobDataSerializerException : Exception
{
    public BackgroundJobDataSerializerException(string message) : base(message)
    {
    }
}