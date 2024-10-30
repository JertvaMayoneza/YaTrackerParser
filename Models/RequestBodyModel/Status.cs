using Newtonsoft.Json;

namespace YaTrackerParser.Models;

public class Status : RequestBodyModel
{
    [JsonProperty("id")]
    public string Id { get; set; }
}