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
            var tokenJson = await File.ReadAllTextAsync(TokenFilePath);
            _token = JsonSerializer.Deserialize<Token>(tokenJson);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (_token == null)
            {
                await LoadToken();
            }
            return _token?.AccessToken;
        }
    }
}
