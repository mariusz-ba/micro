using Microsoft.AspNetCore.Mvc;

namespace Micro.API.Exceptions;

internal interface IExceptionCompositionRoot
{
    ProblemDetails? Map(Exception exception);
}