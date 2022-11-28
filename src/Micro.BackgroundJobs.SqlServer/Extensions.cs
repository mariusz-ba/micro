using Micro.BackgroundJobs.Abstractions;
using Micro.BackgroundJobs.SqlServer.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Micro.BackgroundJobs.SqlServer;

public static class Extensions
{
    public static IServiceCollection AddBackgroundJobs<TContext>(this IServiceCollection services, IConfigurationSection section)
        where TContext : DbContext
    {
        var backgroundJobsOptions = section.Get<BackgroundJobsOptions>();
        
        services.TryAddScoped<IBackgroundJobClient, BackgroundJobClient<TContext>>();
        services.TryAddSingleton<IBackgroundJobDataSerializer, BackgroundJobDataSerializer>();

        services.AddSingleton<IHostedService>(sp => new BackgroundJobProcessor<TContext>(
            sp,
            backgroundJobsOptions,
            sp.GetRequiredService<IBackgroundJobDataSerializer>(),
            sp.GetRequiredService<ILogger<BackgroundJobProcessor<TContext>>>()));

        services.AddSingleton<IHostedService>(sp => new BackgroundJobCleaner<TContext>(
            sp,
            backgroundJobsOptions,
            sp.GetRequiredService<ILogger<BackgroundJobCleaner<TContext>>>()));
        
        return services;
    }
}