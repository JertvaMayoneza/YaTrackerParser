using System.Net.Http.Headers;
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

        public async Task<string> GetTicketsAsync()
        {
            var accessToken = await _tokenManager.GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new UnauthorizedAccessException("Access token is missing or invalid.");
            }

            var client = _httpClientFactory.CreateClient("YaTrackerClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("X-Org-Id", "6442278");

            var response = await client.GetAsync("https://api.tracker.yandex.net/v2/issues");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get tickets. Status code: {response.StatusCode}, Response: {await response.Content.ReadAsStringAsync()}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}