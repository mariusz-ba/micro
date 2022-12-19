using Micro.Common.Exceptions;

namespace Micro.Persistence.Abstractions.Exceptions;

public class EntityDuplicateException : CommonException
{
    public EntityDuplicateException(Exception innerException)
        : base($"Failed to save entity. Given entity is a duplicate.", innerException)
    {
    }
}