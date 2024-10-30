using Newtonsoft.Json;

namespace YaTrackerParser.Models;

public class RequestBody : RequestBodyModel
{
    [JsonProperty("filter")]
    public Filter Filter { get; set; }

    [JsonProperty("order")]
    public string Order { get; set; }
}