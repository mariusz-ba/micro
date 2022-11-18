using Micro.Contexts.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Micro.Contexts.Middleware;

internal sealed class ExecutionContextMiddleware : IMiddleware
{
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public ExecutionContextMiddleware(IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Headers.TryGetValue("trace-id", out var traceId))
        {
            context.TraceIdentifier = traceId;
            
            _executionContextAccessor.Context = new ExecutionContext
            {
                TraceId = context.TraceIdentifier
            };
        }
        else
        {
            context.TraceIdentifier = Guid.NewGuid().ToString("N");
            
            _executionContextAccessor.Context = new ExecutionContext
            {
                TraceId = context.TraceIdentifier
            };
        }

        await next(context);

        context.Response.Headers.TryAdd("trace-id", context.TraceIdentifier);
    }
}