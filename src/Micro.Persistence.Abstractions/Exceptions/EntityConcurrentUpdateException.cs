using Micro.Common.Exceptions;

namespace Micro.Persistence.Abstractions.Exceptions;

public class EntityConcurrentUpdateException : CommonException
{
    public EntityConcurrentUpdateException(Exception innerException)
        : base($"Failed to save entity due to optimistic concurrency check.", innerException)
    {
    }
}