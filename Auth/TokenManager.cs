using System.Text.Json;
using YaTrackerParser.Models;

namespace YaTrackerParser.Auth
{
    public static class TokenManager
    {
        private const string TokenFilePath = "access_token.txt";

        private static async Task<Token> LoadTokenAsync()
        {
            var tokenJson = await File.ReadAllTextAsync(TokenFilePath);
            return JsonSerializer.Deserialize<Token>(tokenJson) ?? throw new Exception($"Не удалось прочитать токен из файла");
        }

        public static async Task<string?> GetAccessTokenAsync()
        {
            var token = await LoadTokenAsync();

            return token.AccessToken;
        }
    }
}
