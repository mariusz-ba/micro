using Micro.Domain.Abstractions.Events;
using Micro.Persistence.Abstractions.UnitOfWork;
using Micro.Persistence.SqlServer.Events;
using Micro.Persistence.SqlServer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            options.UseSqlServer(persistenceOptions.ConnectionString));

        services.AddScoped<IDomainEventProvider, DbContextDomainEventProvider<TContext>>();
        services.AddScoped<IUnitOfWork, DbContextUnitOfWork<TContext>>();

        services.AddHostedService<DatabaseInitializer<TContext>>();
        
        return services;
    }
}