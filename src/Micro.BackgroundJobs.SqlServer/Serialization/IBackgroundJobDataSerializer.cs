using Micro.BackgroundJobs.SqlServer.Persistence;
using System.Linq.Expressions;

namespace Micro.BackgroundJobs.SqlServer.Serialization;

internal interface IBackgroundJobDataSerializer
{
    BackgroundJobData Serialize(LambdaExpression expression);
    BackgroundJobHandlerDescriptor Deserialize(BackgroundJobData data);
}