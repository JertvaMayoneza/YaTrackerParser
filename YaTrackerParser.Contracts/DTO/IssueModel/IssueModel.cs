using Newtonsoft.Json;

namespace YaTrackerParser.Contracts.DTO.IssueModel;

/// <summary>
/// Модель ответа от YandexAPI
/// </summary>
public class Issue
{/// <summary>
/// Код тикета
/// </summary>
    [JsonProperty("key")]
    public string? Key { get; set; } = null;
    /// <summary>
    /// Тема тикета
    /// </summary>
    [JsonProperty("summary")]
    public string? Summary { get; set; } = null;
    /// <summary>
    /// Время последнего апдейта
    /// </summary>
    [JsonProperty("updatedAt")]
    public DateTime? UpdatedAt { get; set; } = null;
    /// <summary>
    /// Описание
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; } = null;
    /// <summary>
    /// Модель для хранения информации об обновлении
    /// </summary>
    [JsonProperty("updatedBy")]
    public UpdatedByInfo? UpdatedBy { get; set; } = null!;
}
