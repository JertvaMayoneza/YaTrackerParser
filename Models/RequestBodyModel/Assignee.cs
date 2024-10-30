using Newtonsoft.Json;

namespace YaTrackerParser.Models;
public class Assignee : RequestBodyModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
}