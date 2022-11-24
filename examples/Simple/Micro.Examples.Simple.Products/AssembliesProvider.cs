using System.Reflection;

namespace Micro.Examples.Simple.Products;

internal static class AssembliesProvider
{
    public static List<Assembly> GetAssemblies() => new()
    {
        typeof(Program).Assembly
    };
}