using System.Text.Json.Serialization;

namespace YaTrackerParser.Contracts.DTO;
/// <summary>
/// Модель токена
/// </summary>
public class Token
{
    /// <summary>
    /// Токен авторизации
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    /// <summary>
    /// Время жизни токена
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    /// <summary>
    /// Обновление токена
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
    /// <summary>
    /// Тип токена
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
    /// <summary>
    /// Оставшееся время жизни токена
    /// </summary>
    public DateTime ExpirationTime { get; set; }
}

