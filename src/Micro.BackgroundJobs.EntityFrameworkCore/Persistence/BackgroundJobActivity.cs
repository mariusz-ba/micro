using Newtonsoft.Json;
using System.Diagnostics;

namespace Micro.BackgroundJobs.EntityFrameworkCore.Persistence;

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
}