using System.Diagnostics;

namespace Micro.Messaging.Abstractions;

public class MessageContext
{
    public string MessageId { get; }
    public string? TraceId { get; }
    public string? ActivityId { get; }
    public Dictionary<string, string?> ActivityTags { get; }
    public Dictionary<string, string?> ActivityBaggage { get; }

    public MessageContext(
        string messageId,
        string? traceId = null,
        string? activityId = null,
        Dictionary<string, string?>? activityTags = null,
        Dictionary<string, string?>? activityBaggage = null)
    {
        MessageId = messageId;
        TraceId = traceId;
        ActivityId = activityId;
        ActivityTags = activityTags ?? new Dictionary<string, string?>();
        ActivityBaggage = activityBaggage ?? new Dictionary<string, string?>();
    }

    public static MessageContext Create()
    {
        if (Activity.Current is null)
        {
            return new MessageContext(Guid.NewGuid().ToString());
        }

        return new MessageContext(
            Guid.NewGuid().ToString(),
            Activity.Current.TraceId.ToString(),
            Activity.Current.Id,
            Activity.Current.Tags.ToDictionary(x => x.Key, x => x.Value),
            Activity.Current.Baggage.ToDictionary(x => x.Key, x => x.Value));
    }
    
    public void Attach(Activity activity)
    {
        if (ActivityId is not null)
        {
            var parts = ActivityId.Split('-');
            if (parts.Length == 4)
            {
                activity.SetParentId(
                    ActivityTraceId.CreateFromString(parts[1]),
                    ActivitySpanId.CreateFromString(parts[2]));
            }
        }
        
        foreach (var pair in ActivityTags)
        {
            activity.AddTag(pair.Key, pair.Value);
        }
        
        foreach (var pair in ActivityBaggage)
        {
            activity.AddBaggage(pair.Key, pair.Value);
        }
    }
}