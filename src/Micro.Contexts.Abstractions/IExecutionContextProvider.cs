namespace Micro.Contexts.Abstractions;

public interface IExecutionContextProvider
{
    IExecutionContext GetContext();
}