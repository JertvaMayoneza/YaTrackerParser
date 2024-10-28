using Newtonsoft.Json;

namespace YaTrackerParser.Models;
/// <summary>
/// Модель ответа от YandexAPI
/// </summary>
public class Issue
{
    [JsonProperty("key")]
    public string? Key { get; set; }

    [JsonProperty("summary")]
    public string? Summary { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
