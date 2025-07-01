using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TFPAW.Models;

namespace TFPAW.Services
{
    // SOLID Principio de Responsabilidad Unica
    // La clase unicamente se encarga de realizar solicitudes a OpenAI pero del tipo chatbot de text
    public class ChatService : IChatBotStrategy 
    {
        // SOLID: Principio de Inversion de Dependencias: Los modulos de alto nivel no dependen de los modulos
        // de bajo nivel y esto se logra mediante la inyeccion de dependencias, es decir no esta asociada a una implementacion concreta
        private readonly HttpClient _httpClient;
        private string _apiKey;
        private readonly IConfiguration _configuration;


        public ChatService(HttpClient httpClient, IConfiguration configuration)
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
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);

                if (response.IsSuccessStatusCode)
                {
                    OpenAIResponse result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                    return result.Choices[0].Message.Content;
                }
                else
                {
                
                    return new Exception($"Error: Unable to get a response from OpenAI. Status Code: {response.StatusCode}");
                }
            }
            catch
            {
                return new Exception("An error has ocurred");
            }
        }
    }
}
