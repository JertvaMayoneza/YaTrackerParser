﻿namespace YaTrackerParser.Middleware;

/// <summary>
/// Класс авторизации подключения к YaTrackerParser API
/// </summary>
public class ApiKeyMiddleware
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Создание экземлпряа ApiKeyMiddleware
    /// </summary>
    /// <param name="next"></param>
    /// <param name="configuration">Файл конфигурации</param>
    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }
    /// <summary>
    /// Метод для првоерки ключа авторизации
    /// </summary>
    /// <param name="context">Информация о HTTP запросе</param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key не предоставлен.");
            return;
        }

        var validApiKey = _configuration["YandexTracker:ApiKeys:Default"]!;

        if (!validApiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Недействительный API Key.");
            return;
        }

        await _next(context);
    }
}