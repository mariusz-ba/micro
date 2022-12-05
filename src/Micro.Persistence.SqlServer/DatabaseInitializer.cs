using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Micro.Persistence.SqlServer;

internal sealed class DatabaseInitializer<TContext> : IHostedService where TContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<PersistenceOptions> _persistenceOptions;

    public DatabaseInitializer(IServiceProvider serviceProvider, IOptions<PersistenceOptions> persistenceOptions)
    {
        _serviceProvider = serviceProvider;
        _persistenceOptions = persistenceOptions;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_persistenceOptions.Value.MigrateDatabase is false)
        {
            return;
        }
        
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}