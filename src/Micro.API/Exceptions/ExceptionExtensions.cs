using Humanizer;
using System.Collections.Concurrent;

namespace Micro.API.Exceptions;

internal static class ExceptionExtensions
{
    private static readonly ConcurrentDictionary<Type, string> ErrorCodes = new();

    public static string Code(this Exception exception)
        => ErrorCodes.GetOrAdd(exception.GetType(), _ => exception.GetType().Name
            .Replace("Exception", string.Empty, StringComparison.InvariantCultureIgnoreCase)
            .Underscore()
            .ToUpper());
}