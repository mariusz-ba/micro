using System.Reflection;

namespace Micro.Messaging.Abstractions.Registration;

internal sealed class SubscribedMessagesProvider : ISubscribedMessagesProvider
{
    public IEnumerable<Type> GetMessagesToSubscribeTo()
        => AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(a =>
                a.IsClass &&
                a.IsAbstract is false &&
                a.IsAssignableTo(typeof(IMessage)) &&
                a.GetCustomAttribute<MessageAttribute>()?.IsExternalMessage is true);
}