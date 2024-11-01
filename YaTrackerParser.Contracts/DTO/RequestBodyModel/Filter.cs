using Newtonsoft.Json;

namespace YaTrackerParser.Contracts.DTO;
/// <summary>
/// Структура для филтрации
/// </summary>
public class Filter : RequestBodyModel
{/// <summary>
/// Имя исполнителя
/// </summary>
    [JsonProperty("assignee")]
    public required Assignee Assignee { get; set; }
    /// <summary>
    /// Статус тикета
    /// </summary>
    [JsonProperty("status")]
    public required Status Status { get; set; }
}