using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using YaTrackerParser.Auth;
using YaTrackerParser.Models;

namespace YaTrackerParser.Services
{
    public class GetTicketsService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetTicketsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Issue>> GetTicketsAsync()
        {
            var result = new List<Issue>();

            var accessToken = await TokenManager.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return result;
            }

            var client = _httpClientFactory.CreateClient("YaTrackerClient");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("X-Org-Id", "6442278");

            var filePath = "application.json";
            var jsonData = await File.ReadAllTextAsync(filePath);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.tracker.yandex.net/v2/issues/_search", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var responseResult = JsonConvert.DeserializeObject<List<Issue>>(jsonResponse);

            if (responseResult != null)
            {
                result = responseResult;
            }

            return result;
        }
    }
}
