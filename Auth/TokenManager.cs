using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YaTrackerParser.Auth
{
    public class TokenManager
    {
        private const string TokenFilePath = "access_token.txt";
        private Token _token;

        private class Token
        {
            public string AccessToken { get; set; }
            public int ExpiresIn { get; set; }
            public string RefreshToken { get; set; }
            public string TokenType { get; set; }
        }

        public TokenManager()
        {
            LoadToken().GetAwaiter().GetResult();
        }

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
                }
                else
                {
                    Console.WriteLine("Token file is empty or missing. Проверьте файл токена.");
                }
            }
            else
            {
                Console.WriteLine("Token file not found. Проверьте файл токена.");
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_token == null || IsTokenExpired())
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
            // Placeholder check for expiration, replace with proper implementation
            return DateTime.UtcNow > DateTime.UtcNow.AddSeconds(_token.ExpiresIn - 3600);
        }

        private async Task RefreshToken()
        {
            Console.WriteLine("Refreshing OAuth token...");
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.yandex.ru/token");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", _token.RefreshToken),
                new KeyValuePair<string, string>("client_id", "b92e4087eab544fa8ec9e175b193b6ef"),
                new KeyValuePair<string, string>("client_secret", "c953f50e8b0d495fa3f3dfab469b70d8")
            });
            request.Content = content;

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
                _token = newToken;
                await File.WriteAllTextAsync(TokenFilePath, JsonSerializer.Serialize(_token));
                Console.WriteLine("OAuth token successfully refreshed and saved.");
            }
            else
            {
                Console.WriteLine("Failed to deserialize new OAuth token. Проверьте файл токена.");
            }
        }
    }
}