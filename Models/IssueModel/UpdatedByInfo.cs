using Newtonsoft.Json;

namespace YaTrackerParser.Models.IssueModel;

/// <summary>
/// Вложенная модель для хранения информации об обновлении
/// </summary>
public class UpdatedByInfo
{/// <summary>
/// Имя обновившего тикет
/// </summary>
    [JsonProperty("display")]
    public string? Display { get; set; } = null;
}