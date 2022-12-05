using Microsoft.AspNetCore.Mvc;

namespace Micro.API.Exceptions;

public abstract class ExceptionToResponseMapperBase : IExceptionToResponseMapper
{
    public abstract ProblemDetails? Map(Exception exception);
    
    protected static ProblemDetails CreateProblemDetails(Exception exception, int status,
        Dictionary<string, object?>? extensions = null)
    {
        var result = new ProblemDetails
        {
            Title = exception.Message,
            Status = status,
            Extensions = { { "code", exception.Code() } }
        };

        if (extensions is null) return result;
        
        foreach (var extension in extensions)
        {
            result.Extensions.Add(extension);
        }

        return result;
    }
}