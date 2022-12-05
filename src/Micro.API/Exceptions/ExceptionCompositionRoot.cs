using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.API.Exceptions;

internal sealed class ExceptionCompositionRoot : IExceptionCompositionRoot
{
    private readonly IServiceProvider _serviceProvider;

    public ExceptionCompositionRoot(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ProblemDetails? Map(Exception exception)
    {
        var mappers = _serviceProvider.GetServices<IExceptionToResponseMapper>().ToArray();
        
        var result = mappers
            .Where(m => m is not ExceptionToResponseMapper)
            .Select(m => m.Map(exception))
            .FirstOrDefault(e => e is not null);

        if (result is not null)
        {
            return result;
        }

        var mapperFallback = mappers.SingleOrDefault(m => m is ExceptionToResponseMapper);
        return mapperFallback?.Map(exception);
    }
}