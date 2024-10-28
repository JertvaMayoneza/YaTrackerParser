using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using YaTrackerParser.Auth;
using YaTrackerParser.Models;
using static YaTrackerParser.Models.RequestBodyModel;

/// <summary>
/// Серис для получения тикетов от YandexAPI
/// </summary>
public class GetTicketsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    
    public GetTicketsService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
    /// <summary>
    /// Метод для получения тикетов из Yandex Tracker
    /// </summary>
    /// <returns>Список тикетов</returns>
    /// <exception cref="Exception">Пустой ответ от сервера</exception>
    public async Task<List<Issue>> GetTicketsAsync()
    {
        var result = new List<Issue>();
        var token = TokenManager.GetToken();

        if (string.IsNullOrWhiteSpace(token?.AccessToken))
        {
            Console.WriteLine("Access token is null or empty.");
            return result;
        }

        var client = _httpClientFactory.CreateClient("YaTrackerClient");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        client.DefaultRequestHeaders.Add("X-Org-Id", _configuration["YandexTracker:OrgId"]);

        var requestBodySection = _configuration.GetSection("YandexTracker:RequestBody");
        var jsonRequestBody = JsonConvert.SerializeObject(requestBodySection.Get<RequestBody>());

        Console.WriteLine(jsonRequestBody);

        var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api.tracker.yandex.net/v2/issues/_search", content);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var responceResult = JsonConvert.DeserializeObject<List<Issue>>(jsonResponse)
            ?? throw new Exception("Пустой ответ от сервера");

        return responceResult;
    }
}
