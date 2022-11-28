namespace Micro.BackgroundJobs.SqlServer.Serialization;

internal class BackgroundJobDataSerializerException : Exception
{
    public BackgroundJobDataSerializerException(string message) : base(message)
    {
    }
}