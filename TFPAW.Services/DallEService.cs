using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TFPAW.Models;

namespace TFPAW.Services
{
    public class DallEService : IChatBotStrategy
    {
        private readonly HttpClient _httpClient;
        private string _apiKey;
        private readonly IConfiguration _configuration;

        public DallEService(HttpClient httpClient, IConfiguration configuration)
        {
            _apiKey = "";
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<object> GetResponseAsync(string message)
        {
            _apiKey = _configuration.GetSection("ApiKeys:OpenAI").Value;

            if (string.IsNullOrEmpty(_apiKey))
            {
                return new Exception("Api Key is Empty");
            }


            object request = new
            {
                model = "dall-e-3",
                prompt = message,
                n = 1,
                size = "1024x1024"
            };

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/images/generations", request);

                if (response.IsSuccessStatusCode)
                {
                    DallEResponse result = await response.Content.ReadFromJsonAsync<DallEResponse>();
                    return result.data[0];
                }
                else
                {
                    return new Exception($"Error: Unable to get a response from DallE. Status Code: {response.StatusCode}");
                }
            }
            catch
            {
                return new Exception("An error has ocurred");
            }
        }
    }
}
