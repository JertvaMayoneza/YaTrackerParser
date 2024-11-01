using Newtonsoft.Json;

namespace YaTrackerParser.Contracts.DTO;
/// <summary>
/// Имя исполнителя
/// </summary>
public class Assignee : RequestBodyModel
{
    /// <summary>
    /// Имя исполнителя
    /// </summary>
    [JsonProperty("id")]
    public required string Id { get; set; }
}