using Micro.BackgroundJobs.EntityFrameworkCore.Persistence;
using System.Linq.Expressions;

namespace Micro.BackgroundJobs.EntityFrameworkCore.Serialization;

internal interface IBackgroundJobDataSerializer
{
    BackgroundJobData Serialize(LambdaExpression expression);
    BackgroundJobHandlerDescriptor Deserialize(BackgroundJobData data);
}