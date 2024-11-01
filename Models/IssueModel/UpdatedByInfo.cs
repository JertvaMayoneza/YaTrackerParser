using Newtonsoft.Json;

namespace YaTrackerParser.Models.IssueModel;

/// <summary>
/// Вложенная модель для хранения информации об обновлении
/// </summary>
public class UpdatedByInfo
{
    [JsonProperty("display")]
    public string? Display { get; set; }
}