using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;

namespace Micro.API.Networking;

public static class Extensions
{
    public static IServiceCollection AddHeadersForwarding(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<NetworkingOptions>(configuration.GetSection(NetworkingOptions.SectionName));

        return services;
    }

    public static IApplicationBuilder UseHeadersForwarding(this IApplicationBuilder app)
    {
        var networkingOptions = app.ApplicationServices.GetRequiredService<IOptions<NetworkingOptions>>();

        app.Use((context, next) =>
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix))
            {
                context.Request.PathBase = new PathString($"/{prefix}");
            }

            return next(context);
        });

        app.UseForwardedHeaders(CreateForwardedHeadersOptions(networkingOptions.Value));
        
        return app;
    }

    private static ForwardedHeadersOptions CreateForwardedHeadersOptions(NetworkingOptions networkingOptions)
    {
        var forwardedHeadersOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All,
            ForwardLimit = null
        };

        foreach (var knownNetwork in networkingOptions.KnownNetworks)
        {
            forwardedHeadersOptions.KnownNetworks.Add(new IPNetwork(IPAddress.Parse(knownNetwork.Prefix), knownNetwork.PrefixLength));
        }

        return forwardedHeadersOptions;
    }
}