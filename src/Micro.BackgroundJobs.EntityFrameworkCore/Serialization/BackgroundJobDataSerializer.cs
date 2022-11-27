using Micro.BackgroundJobs.EntityFrameworkCore.Persistence;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Micro.BackgroundJobs.EntityFrameworkCore.Serialization;

internal sealed class BackgroundJobDataSerializer : IBackgroundJobDataSerializer
{
    private static readonly ConcurrentDictionary<string, Type?> StringToType = new();
    
    public BackgroundJobData Serialize(LambdaExpression expression)
    {
        var handlerType = GetHandlerType(expression);
        var handlerMethod = GetHandlerMethod(expression);
        var handlerMethodArguments = GetHandlerMethodArguments(expression);

        return new BackgroundJobData
        {
            HandlerType = handlerType.FullName ?? throw new BackgroundJobDataSerializerException("Missing handler type name"),
            HandlerGenericArguments = handlerType
                .GetGenericArguments()
                .Select(t => t.FullName ?? throw new BackgroundJobDataSerializerException("Missing handler generic argument type name."))
                .ToArray(),
            HandlerMethod = handlerMethod.IsGenericMethodDefinition || handlerMethod.IsGenericMethod
                ? handlerMethod.GetGenericMethodDefinition().ToString() ?? handlerMethod.Name
                : handlerMethod.ToString() ?? handlerMethod.Name,
            HandlerMethodGenericArguments = handlerMethod
                .GetGenericArguments()
                .Select(t => t.FullName ?? throw new BackgroundJobDataSerializerException("Missing handler method generic argument type."))
                .ToArray(),
            HandlerMethodParameters = handlerMethodArguments,
            Activity = BackgroundJobActivity.CreateFromCurrent()
        };
    }

    public BackgroundJobHandlerDescriptor Deserialize(BackgroundJobData data)
    {
        var handlerType = GetTypeByFullName(data.HandlerType) ??
                          throw new BackgroundJobDataSerializerException("Could not find target handler type.");

        if (data.HandlerGenericArguments.Any())
        {
            handlerType = handlerType
                .MakeGenericType(data.HandlerGenericArguments.Select(a =>
                    GetTypeByFullName(a) ??
                    throw new BackgroundJobDataSerializerException(
                        "Could not find target handler generic argument type.")).ToArray());
        }

        var handlerMethod = handlerType.GetMethods().FirstOrDefault(m =>
                                m.ToString() == data.HandlerMethod || m.Name == data.HandlerMethod) ??
                            throw new BackgroundJobDataSerializerException("Could not find target handler method.");

        if (data.HandlerMethodGenericArguments.Any())
        {
            handlerMethod = handlerMethod
                .MakeGenericMethod(data.HandlerMethodGenericArguments.Select(a =>
                    GetTypeByFullName(a) ??
                    throw new BackgroundJobDataSerializerException(
                        "Could not find target handler method generic argument type.")).ToArray());
        }

        return new BackgroundJobHandlerDescriptor(
            handlerType,
            handlerMethod,
            data.HandlerMethodParameters,
            data.Activity);
    }
    
    private static Type GetHandlerType(Expression expression) =>
        expression.Type.GenericTypeArguments.FirstOrDefault() ??
        throw new BackgroundJobDataSerializerException("Could not obtain handler type.");
    
    private static MethodInfo GetHandlerMethod(LambdaExpression expression)
    {
        if (expression.Body is not MethodCallExpression methodExpression)
        { 
            throw new BackgroundJobDataSerializerException("Could not obtain handler method.");
        }
        return methodExpression.Method;
    }

    private static object?[] GetHandlerMethodArguments(LambdaExpression expression)
    {
        if (expression.Body is not MethodCallExpression methodExpression)
        {
            throw new BackgroundJobDataSerializerException("Could not obtain handler method.");
        }

        var arguments = methodExpression.Arguments
            .Select(argument => ((argument as MemberExpression)?.Expression as ConstantExpression)?.Value)
            .Where(argument => argument is not null)
            .SelectMany(argument => argument!.GetType().GetFields().Select(f => f.GetValue(argument)))
            .ToArray();

        if (arguments.Length != methodExpression.Arguments.Count)
        {
            throw new BackgroundJobDataSerializerException("Could not obtain handler method arguments.");
        }

        return arguments;
    }

    private static Type? GetTypeByFullName(string fullName) => StringToType
        .GetOrAdd(fullName, _ => AppDomain.CurrentDomain
            .GetAssemblies().SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.FullName == fullName));
}
