using Newtonsoft.Json;

namespace Micro.BackgroundJobs.EntityFrameworkCore.Persistence.Converters;

internal static class ClassToJsonConverterOptions
{
    internal static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto
    };
}