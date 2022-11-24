using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;

namespace Micro.Contexts.Middleware;

internal sealed class ExecutionContextUserMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userId is not null)
        {
            Activity.Current?.SetTag("UserId", userId.Value);
        }

        return next(context);
    }
}