using Micro.Contexts.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Micro.Contexts.Middleware;

internal sealed class IdentityContextMiddleware : IMiddleware
{
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public IdentityContextMiddleware(IExecutionContextAccessor executionContextAccessor)
    {
        _executionContextAccessor = executionContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_executionContextAccessor.Context is null)
        {
            await next(context);
            return;
        }
        
        if (context.Request.Headers.TryGetValue("user-id", out var userId))
        {
            if (_executionContextAccessor.Context is ExecutionContext executionContext)
            {
                executionContext.UserId = userId;
            }
        }

        await next(context);
    }
}