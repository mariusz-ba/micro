using Micro.Common.Exceptions;
using Micro.Domain.Abstractions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.API.Exceptions;

internal sealed class ExceptionToResponseMapper : ExceptionToResponseMapperBase
{
    public override ProblemDetails? Map(Exception exception)
        => exception switch
        {
            EntityNotFoundException ex => CreateProblemDetails(ex, StatusCodes.Status404NotFound),
            CommonException ex => CreateProblemDetails(ex, StatusCodes.Status400BadRequest),
            _ => null
        };
}