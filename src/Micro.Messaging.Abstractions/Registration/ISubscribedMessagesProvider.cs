namespace Micro.Messaging.Abstractions.Registration;

public interface ISubscribedMessagesProvider
{
    IEnumerable<Type> GetMessagesToSubscribeTo();
}