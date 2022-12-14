namespace Micro.CQRS.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command);
}