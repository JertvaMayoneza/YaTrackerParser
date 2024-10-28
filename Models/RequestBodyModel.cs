using Newtonsoft.Json;

namespace YaTrackerParser.Models;
/// <summary>
/// Модель запроса к YandexaAPI
/// </summary>
public class RequestBodyModel
{
    public class Assignee
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Status
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Filter
    {
        [JsonProperty("assignee")]
        public Assignee Assignee { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }

    public class RequestBody
    {
        [JsonProperty("filter")]
        public Filter Filter { get; set; }

        [JsonProperty("order")]
        public string Order { get; set; }
    }
}
