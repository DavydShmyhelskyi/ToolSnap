namespace AiService.Services
{
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;

    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string apiKey = "AIzaSyD4J45ZDZhwLE72vkUGFxDAUZHe0wDOLV8";

        public GeminiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<string> GetChatResponseWithImage(
    string? prompt,
    byte[]? imageBytes,
    string? mimeType)
        {
            var parts = new List<object>();

            if (!string.IsNullOrWhiteSpace(prompt))
                parts.Add(new { text = prompt });

            if (imageBytes != null && mimeType != null)
            {
                parts.Add(new
                {
                    inline_data = new
                    {
                        mime_type = mimeType,
                        data = Convert.ToBase64String(imageBytes)
                    }
                });
            }

            var requestBody = new
            {
                contents = new[]
                {
            new { parts }
        }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent"
            );

            request.Headers.Add("X-goog-api-key", apiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(stream);

            return json.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "No response";
        }



        public async Task<string> getChatResponse(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
            };


            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent"
            );

            request.Headers.Add("X-goog-api-key", apiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseContent = await response.Content.ReadAsStreamAsync();
                using var jsonDoc = await JsonDocument.ParseAsync(responseContent);

                var text = jsonDoc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? "No response from Gemini";
            }
            else
            {
                throw new HttpRequestException(
                    $"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}"
                );
            }
        }
    }
}
