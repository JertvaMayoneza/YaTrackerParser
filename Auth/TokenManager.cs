using YaTrackerParser.Contracts.DTO;

namespace YaTrackerParser.Auth;

/// <summary>
/// Класс для обработки токена
/// </summary>
public static class TokenManager
{
    private static IConfiguration? _configuration;
    /// <summary>
    /// Инициализация токена
    /// </summary>
    /// <param name="configuration"></param>
    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// Получение токена из конфигурации
    /// </summary>
    /// <returns>Возвращает экземпляр токена</returns>
    /// <exception cref="Exception">Не удалось инициализировать конфигурацию</exception>
    public static Token GetToken()
    {
        if (_configuration == null)
            throw new Exception("Конфигурация не инициализирована");

        var tokenSection = _configuration.GetSection("YandexTrackerAuth");

        return new Token
        {
            AccessToken = tokenSection["access_token"],
        };
    }
}