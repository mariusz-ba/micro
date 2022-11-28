using Newtonsoft.Json;
using System.Diagnostics;

namespace Micro.BackgroundJobs.SqlServer.Persistence;

internal sealed class BackgroundJobActivity
{
    public string? ActivityId { get; }
    public Dictionary<string, string?> ActivityTags { get; }
    public Dictionary<string, string?> ActivityBaggage { get; }

    [JsonConstructor]
    private BackgroundJobActivity(
        string? activityId,
        Dictionary<string, string?> activityTags,
        Dictionary<string, string?> activityBaggage)
    {
        ActivityId = activityId;
        ActivityTags = activityTags;
        ActivityBaggage = activityBaggage;
    }

    public static BackgroundJobActivity? CreateFromCurrent()
    {
        if (Activity.Current is null)
        {
            return null;
        }

        return new BackgroundJobActivity(
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