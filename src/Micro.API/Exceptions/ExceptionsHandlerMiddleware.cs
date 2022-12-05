using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Micro.API.Exceptions;

internal sealed class ExceptionsHandlerMiddleware : IMiddleware
{
    private readonly IWebHostEnvironment _environment;
    private readonly IExceptionCompositionRoot _exceptionCompositionRoot;
    private readonly ILogger<ExceptionsHandlerMiddleware> _logger;

    public ExceptionsHandlerMiddleware(
        IWebHostEnvironment environment,
        IExceptionCompositionRoot exceptionCompositionRoot,
        ILogger<ExceptionsHandlerMiddleware> logger)
    {
        _environment = environment;
        _exceptionCompositionRoot = exceptionCompositionRoot;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, exception.Message);
        
        var response = new ProblemDetails
        {
            Title = "Internal server error",
            Status = StatusCodes.Status500InternalServerError,
            Extensions =
            {
                { "code", "INTERNAL_SERVER_ERROR" }
            }
        };

        var result = _exceptionCompositionRoot.Map(exception);
        if (result is not null)
        {
            response = result;
        }
        
        if (_environment.IsDevelopment())
        {
            response.Extensions.Add("exception", new
            {
                exception.Message,
                exception.StackTrace
            });
        }

        context.Response.StatusCode = response.Status ?? 500;
        await context.Response.WriteAsJsonAsync(response);
    }
}