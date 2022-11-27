using Micro.BackgroundJobs.EntityFrameworkCore.Persistence;
using System.Reflection;

namespace Micro.BackgroundJobs.EntityFrameworkCore;

internal sealed class BackgroundJobHandlerDescriptor
{
    public Type HandlerType { get; }
    public MethodInfo HandlerMethod { get; }
    public object?[] HandlerMethodParameters { get; }
    public BackgroundJobActivity? Activity { get; }

    public BackgroundJobHandlerDescriptor(
        Type handlerType,
        MethodInfo handlerMethod,
        object?[] handlerMethodParameters,
        BackgroundJobActivity? activity)
    {
        HandlerType = handlerType;
        HandlerMethod = handlerMethod;
        HandlerMethodParameters = handlerMethodParameters;
        Activity = activity;
    }
}