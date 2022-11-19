using Micro.Common.App;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Common;

public static class Extensions
{
    public static IServiceCollection AddMicro(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppOptions(configuration);
        
        return services;
    }
}