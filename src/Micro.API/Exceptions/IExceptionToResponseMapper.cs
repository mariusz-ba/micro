using Microsoft.AspNetCore.Mvc;

namespace Micro.API.Exceptions;

public interface IExceptionToResponseMapper
{
    ProblemDetails? Map(Exception exception);
}