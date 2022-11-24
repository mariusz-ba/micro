using Micro.Common.Abstractions.Exceptions;
using System.Reflection;

namespace Micro.Domain.Abstractions.Exceptions;

public class EntityNotFoundException : CommonException
{
    public EntityNotFoundException(MemberInfo type, string id)
        : base($"Entity of type '{type.Name}' with ID '{id}' does not exist.")
    {
    }
}