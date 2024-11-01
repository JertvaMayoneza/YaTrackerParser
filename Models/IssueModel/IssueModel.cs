using Newtonsoft.Json;

namespace YaTrackerParser.Models.IssueModel;

/// <summary>
/// Модель ответа от YandexAPI
/// </summary>
public class Issue
{
    [JsonProperty("key")]
    public string? Key { get; set; } = null;

    [JsonProperty("summary")]
    public string? Summary { get; set; } = null;

    [JsonProperty("updatedAt")]
    public DateTime? UpdatedAt { get; set; } = null;

    [JsonProperty("description")]
    public string? Description { get; set; } = null;

    [JsonProperty("updatedBy")]
    public UpdatedByInfo? UpdatedBy { get; set; } = null;
}
