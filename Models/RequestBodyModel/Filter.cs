using Newtonsoft.Json;

namespace YaTrackerParser.Models;

public class Filter : RequestBodyModel
{
    [JsonProperty("assignee")]
    public Assignee Assignee { get; set; }

    [JsonProperty("status")]
    public Status Status { get; set; }
}