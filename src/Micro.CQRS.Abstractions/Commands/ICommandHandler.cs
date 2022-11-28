namespace Micro.CQRS.Abstractions.Commands;

public interface ICommandHandler<in TCommand, TResult>
    where TCommand : class, ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command);
}

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : class, ICommand<Unit>
{
}