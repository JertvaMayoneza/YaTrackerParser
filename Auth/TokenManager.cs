using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
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
                Console.WriteLine($"Token JSON content: {tokenJson}");

                if (!string.IsNullOrEmpty(tokenJson))
                {
                    _token = JsonSerializer.Deserialize<Token>(tokenJson);
                    if (_token == null)
                    {
                        Console.WriteLine("Failed to deserialize token.");
                    }
                    else
                    {
                        Console.WriteLine($"Deserialized Access Token: {_token.AccessToken}");
                    }
                }
                else
                {
                    Console.WriteLine("Token file is empty or missing.");
                }
            }
            else
            {
                Console.WriteLine("Token file does not exist.");
            }
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
