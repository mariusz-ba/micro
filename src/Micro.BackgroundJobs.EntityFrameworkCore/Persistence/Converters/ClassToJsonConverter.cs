using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Micro.BackgroundJobs.EntityFrameworkCore.Persistence.Converters;

internal sealed class ClassToJsonConverter<T> : ValueConverter<T?, string?> where T : class
{
    public ClassToJsonConverter() : base(
        value => JsonConvert.SerializeObject(value, Formatting.None, ClassToJsonConverterOptions.SerializerSettings),
        value => value == null
            ? null
            : JsonConvert.DeserializeObject<T>(value, ClassToJsonConverterOptions.SerializerSettings))
    {
    }
}