using Micro.Common.Abstractions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.API.Exceptions;

internal sealed class ExceptionToResponseMapper : ExceptionToResponseMapperBase
{
    public override ProblemDetails? Map(Exception exception)
        => exception switch
        {
            CommonException ex => CreateProblemDetails(ex, StatusCodes.Status400BadRequest),
            _ => null
        };
}