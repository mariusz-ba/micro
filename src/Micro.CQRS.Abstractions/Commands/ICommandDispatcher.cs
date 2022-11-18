namespace Micro.CQRS.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
}