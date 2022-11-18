using System.Reflection;

namespace Micro.Examples.Web;

public static class AssembliesProvider
{
    public static IList<Assembly> GetAssemblies() => new List<Assembly>
    {
        typeof(Program).Assembly
    };
}