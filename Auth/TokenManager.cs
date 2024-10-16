using System.Text.Json;
using YaTrackerParser.Models;

namespace YaTrackerParser.Auth
{
    public class TokenManager
    {
        private const string TokenFilePath = "access_token.txt";
        private Token _token;

        public TokenManager() => LoadToken().GetAwaiter().GetResult();

        private async Task LoadToken()
        {
            Console.WriteLine("Loading access token from file...");
            if (File.Exists(TokenFilePath))
            {
                var tokenJson = await File.ReadAllTextAsync(TokenFilePath);
                if (!string.IsNullOrEmpty(tokenJson))
                {
                    _token = JsonSerializer.Deserialize<Token>(tokenJson);
                    if (_token == null)
                    {
                        Console.WriteLine("Failed to deserialize token. Проверьте файл токена.");
                    }
                    else
                    {
                        _token.ExpirationTime = DateTime.UtcNow.AddSeconds(_token.ExpiresIn);
                    }
                }
                else
                {
                    Console.WriteLine("Token file is empty or missing. Проверьте файл токена.");
                }
            }
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (IsTokenExpired())
            {
                Console.WriteLine("Token is expired or missing, refreshing token...");
                await RefreshToken();
            }
            return _token?.AccessToken;
        }

        private bool IsTokenExpired()
        {
            if (_token == null)
            {
                return true;
            }

            return DateTime.UtcNow >= _token.ExpirationTime;
        }

        private async Task RefreshToken()
        {
            if (_token?.RefreshToken == null)
            {
                Console.WriteLine("No refresh token available. Проверьте файл токена.");
                return;
            }

            Console.WriteLine("Refreshing OAuth token...");
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.yandex.ru/token");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", _token.RefreshToken),
                new KeyValuePair<string, string>("client_id", "b51d2235324e4df49b37e2f471f7e139"),
                new KeyValuePair<string, string>("client_secret", "8211ba2d3bef4ae19b9dd65d52279af3")
            });

            request.Content = content;

            try
            {
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to refresh OAuth token. Status code: {response.StatusCode}");
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var newToken = JsonSerializer.Deserialize<Token>(responseContent);

                if (newToken != null)
                {
                    newToken.ExpirationTime = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn);

                    _token = newToken;
                    await File.WriteAllTextAsync(TokenFilePath, JsonSerializer.Serialize(_token));

                    Console.WriteLine("OAuth token successfully refreshed and saved.");
                }
                else
                {
                    Console.WriteLine("Failed to deserialize new OAuth token. Проверьте файл токена.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while refreshing token: {ex.Message}");
            }
        }
    }
}
