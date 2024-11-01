using Newtonsoft.Json;

namespace YaTrackerParser.Contracts.DTO;
/// <summary>
/// Статус тикета
/// </summary>
public class Status : RequestBodyModel
{/// <summary>
/// Статус тикета
/// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }
}