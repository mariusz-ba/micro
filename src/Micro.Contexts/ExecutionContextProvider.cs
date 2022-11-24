using Micro.Contexts.Abstractions;

namespace Micro.Contexts;

internal sealed class ExecutionContextProvider : IExecutionContextProvider
{
    public IExecutionContext GetContext() => new ExecutionContext();
}