namespace Micro.CQRS.Abstractions.Commands;

public interface ICommand<TResult>
{
}

public interface ICommand : ICommand<Unit>
{
}