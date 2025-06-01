using System.Text;
using System.Text.Json;

namespace NovaVoice.Services;

public class GeminiService
{
    private const string ApiKey = "AIzaSyBiRyMpUwUO2ZUNBb8Bhg4eWOwwdxbqjFY";
    private const string ApiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={ApiKey}";

    private readonly HttpClient _httpClient;

    public GeminiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetGeminiReplyAsync(string userInput)
    {
        var payload = new
        {
            system_instruction = new
            {
                parts = new[] {
                    new { text = "You are Niko, the virtual AI girlfriend of Swapnil Gaikwad..." }
                }
            },
            contents = new[]
            {
                new { parts = new[] { new { text = userInput } } }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(ApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonDocument.Parse(responseBody);
                return result.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text").GetString();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Gemini Error: " + ex.Message);
        }

        return "Something went wrong with the AI.";
    }
}
