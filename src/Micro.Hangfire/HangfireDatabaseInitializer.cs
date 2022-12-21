using Hangfire.SqlServer;
using Micro.Persistence.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Micro.Hangfire;

internal sealed class HangfireDatabaseInitializer : IHostedService
{
    private readonly IOptions<PersistenceOptions> _persistenceOptions;
    private readonly IOptions<HangfireOptions> _hangfireOptions;

    public HangfireDatabaseInitializer(
        IOptions<PersistenceOptions> persistenceOptions,
        IOptions<HangfireOptions> hangfireOptions)
    {
        _persistenceOptions = persistenceOptions;
        _hangfireOptions = hangfireOptions;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_hangfireOptions.Value.MigrateDatabase is false)
        {
            return;
        }

        await using var connection = new SqlConnection(_persistenceOptions.Value.ConnectionString);
        SqlServerObjectsInstaller.Install(connection, "HangFire");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}