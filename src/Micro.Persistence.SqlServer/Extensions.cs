using Micro.Domain.Abstractions.Events;
using Micro.Persistence.Abstractions.UnitOfWork;
using Micro.Persistence.SqlServer.Events;
using Micro.Persistence.SqlServer.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Micro.Persistence.SqlServer;

public static class Extensions
{
    public static IServiceCollection AddPersistence<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        var persistenceSection = configuration.GetSection(PersistenceOptions.SectionName);
        var persistenceOptions = persistenceSection.Get<PersistenceOptions>();

        services.Configure<PersistenceOptions>(persistenceSection);

        services.AddDbContext<TContext>(options =>
                options.UseSqlServer(persistenceOptions.ConnectionString),
            optionsLifetime: ServiceLifetime.Singleton);

        services.AddDbContextFactory<TContext>(options =>
            options.UseSqlServer(persistenceOptions.ConnectionString));

        services.AddScoped<IDomainEventProvider, DbContextDomainEventProvider<TContext>>();
        services.AddScoped<IUnitOfWork, DbContextUnitOfWork<TContext>>();

        return services;
    }

    public static IApplicationBuilder MigrateDatabase<TContext>(this IApplicationBuilder app) where TContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        dbContext.Database.Migrate();
        
        return app;
    }
}