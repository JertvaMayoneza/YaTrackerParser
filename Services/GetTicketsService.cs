using System.Net.Http.Headers;
using System.Text;
using YaTrackerParser.Auth;

namespace YaTrackerParser.Services
{
    public class GetTicketsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenManager _tokenManager;

        public GetTicketsService(IHttpClientFactory httpClientFactory, TokenManager tokenManager)
        {
            _httpClientFactory = httpClientFactory;
            _tokenManager = tokenManager;
        }
        
        public async Task<string> SendPostRequestAsync()
        {
            var accessToken = await _tokenManager.GetAccessTokenAsync();
            var client = _httpClientFactory.CreateClient("YaTrackerClient");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("X-Org-Id", "6442278");

            var filePath = "application.json";
            var jsonData = await File.ReadAllTextAsync(filePath);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.tracker.yandex.net/v2/issues/_search", content);

            return await response.Content.ReadAsStringAsync();

        }

    }
}