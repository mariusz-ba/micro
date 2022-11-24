namespace Micro.Persistence.SqlServer;

internal class PersistenceOptions
{
    public const string SectionName = "Persistence";
    public string ConnectionString { get; set; } = string.Empty;
}