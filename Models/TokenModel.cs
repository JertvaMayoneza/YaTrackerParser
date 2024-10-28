﻿using System.Text.Json.Serialization;
/// <summary>
/// Модель токена
/// </summary>
public class Token
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    public DateTime ExpirationTime { get; set; }
}

