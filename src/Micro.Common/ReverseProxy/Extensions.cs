using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;

namespace Micro.Common.ReverseProxy;

public static class Extensions
{
    public static IApplicationBuilder UseReverseProxyHeaders(this IApplicationBuilder app)
    {
        app.Use((context, next) =>
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix))
            {
                context.Request.PathBase = new PathString($"/{prefix}");
            }

            return next(context);
        });

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        return app;
    }
}