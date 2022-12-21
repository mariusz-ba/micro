namespace Micro.Persistence.SqlServer;

public sealed class PersistenceOptions
{
    public const string SectionName = "Persistence";
    public string ConnectionString { get; set; } = string.Empty;
    public bool MigrateDatabase { get; set; }
}