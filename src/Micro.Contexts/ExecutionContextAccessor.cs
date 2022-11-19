using Micro.Contexts.Abstractions;

namespace Micro.Contexts;

internal sealed class ExecutionContextAccessor : IExecutionContextAccessor
{
    private static readonly AsyncLocal<ContextHolder> Holder = new();
    
    public IExecutionContext? Context
    {
        get => Holder.Value?.Context;
        set
        {
            var holder = Holder.Value;
            if (holder is not null)
            {
                holder.Context = null;
            }

            if (value is not null)
            {
                Holder.Value = new ContextHolder { Context = value };
            }
        }
    }

    private class ContextHolder
    {
        public IExecutionContext? Context;
    }
}