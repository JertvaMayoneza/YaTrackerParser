using Newtonsoft.Json;

namespace YaTrackerParser.Models;
/// <summary>
/// Модель тела запроса
/// </summary>
public class RequestBody : RequestBodyModel
{/// <summary>
/// Фильтры запроса
/// </summary>
    [JsonProperty("filter")]
    public Filter Filter { get; set; }
    /// <summary>
    /// Сортировка тикетов
    /// </summary>
    [JsonProperty("order")]
    public string Order { get; set; }
}