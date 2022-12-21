using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire;

namespace Micro.Hangfire.Messaging;

internal sealed class MessageJobDisplayNameAttribute : JobDisplayNameAttribute
{
    public MessageJobDisplayNameAttribute(string displayName) : base(displayName)
    {
    }

    public override string Format(DashboardContext context, Job job)
        => string.Format(DisplayName, job.Args.Select(arg =>
        {
            var argumentType = arg.GetType();
            return argumentType.IsGenericType
                ? (object)argumentType.GetGenericArguments().First().Name
                : (object)argumentType.Name;
        }).ToArray());
}