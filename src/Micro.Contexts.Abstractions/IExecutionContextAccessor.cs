namespace Micro.Contexts.Abstractions;

public interface IExecutionContextAccessor
{
    public IExecutionContext? Context { get; set; }
}