using Hangfire.SqlServer;
using Hangfire;
using Micro.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.Hangfire;

public static class Extensions
{
    public static IServiceCollection AddHangfire<TContext>(this IServiceCollection services,
        IConfiguration configuration)
        where TContext : DbContext
    {
        var persistenceOptions = configuration.GetSection(PersistenceOptions.SectionName).Get<PersistenceOptions>();

        var hangfireSection = configuration.GetSection(HangfireOptions.SectionName);
        var hangfireOptions = hangfireSection.Get<HangfireOptions>();

        services.Configure<HangfireOptions>(hangfireSection);

        services.AddHostedService<HangfireDatabaseInitializer>();

        var sqlServerStorageOptions = new SqlServerStorageOptions
        {
            QueuePollInterval = hangfireOptions.Storage?.QueuePollInterval ?? TimeSpan.Zero,
            CommandBatchMaxTimeout = hangfireOptions.Storage?.CommandBatchMaxTimeout ?? TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = hangfireOptions.Storage?.SlidingInvisibilityTimeout ?? TimeSpan.FromMinutes(5),
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true,
            PrepareSchemaIfNecessary = false
        };

        services.AddHangfire(globalConfiguration => globalConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(persistenceOptions.ConnectionString, sqlServerStorageOptions));
        
        services.AddBackgroundJobClientWithSharedDbConnection<TContext>(sqlServerStorageOptions);

        foreach (var server in hangfireOptions.Servers ?? Enumerable.Empty<HangfireServerOptions>())
        {
            services.AddHangfireServer(options =>
            {
                if (server.WorkerCount.HasValue)
                {
                    options.WorkerCount = server.WorkerCount.Value;
                }

                if (server.Queues is not null)
                {
                    options.Queues = server.Queues;
                }
            });
        }

        return services;
    }

    private static void AddBackgroundJobClientWithSharedDbConnection<TContext>(this IServiceCollection services,
        SqlServerStorageOptions storageOptions)
        where TContext : DbContext
    {
        var backgroundJobClient = services.FirstOrDefault(d => d.ServiceType == typeof(IBackgroundJobClient));
        if (backgroundJobClient is not null)
        {
            services.Remove(backgroundJobClient);
        }

        services.AddScoped<IBackgroundJobClient>(sp => new BackgroundJobClient(
            new SqlServerStorage(sp.GetRequiredService<TContext>().Database.GetDbConnection(), storageOptions)));
    }
}