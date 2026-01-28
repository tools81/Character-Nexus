using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class GenerateImageWithAI
    {
        private static string apiKey = "your_api_key"; // Replace with your actual API key

        public static async Task<string> GenerateAndReturnUrl(string userPrompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var apiUrl = "https://api.openai.com/v1/engines/dall-e-3/queries";
                var requestBody = new { prompt = userPrompt, max_tokens = 100 }; // Customize as needed

                var response = await client.PostAsJsonAsync(apiUrl, requestBody);
                var content = await response.Content.ReadAsStringAsync();

                // Parse the JSON response to get the image URL or other relevant data
                var jsonResponse = JObject.Parse(content);
                var imageUrl = jsonResponse["choices"][0]["text"].ToString();

                return imageUrl;
            }
        }
    }
}
