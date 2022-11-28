using Newtonsoft.Json;

namespace Micro.BackgroundJobs.SqlServer.Persistence.Converters;

internal static class ClassToJsonConverterOptions
{
    internal static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto
    };
}