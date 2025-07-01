using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TFPAW.Models;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Extensions.Configuration;

namespace TFPAW.Services
{
    // SOLID: Principio de abierto/cerrado: Esta clase podria extender su funcionalidad sin modificar los metodos actuales
    public class PdfChatService : IPdfChatService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private string _apiKey;  
        public PdfChatService(HttpClient httpClient, IConfiguration configuration)
        {
            
            _apiKey = "";
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> ProcessPdfAndRespond(IFormFileCollection files, string question)
        {

            try
            {
                if (files == null || files.Count == 0)
                {
                    throw new ArgumentException("No files uploaded");
                }

                var fileContents = await Task.WhenAll(files.Select(ExtractTextFromPdfAsync));
                var combinedContent = string.Join("---\n", fileContents);
                var chunks = SplitIntoChunks(combinedContent);
                var fullResponse = new StringBuilder();

                foreach (var chunk in chunks)
                {
                    var prompt = $"Please analyze the following part of the document(s) and answer this question: {question}\n\n{chunk}";
                    var chunkResponse = await GetResponseAsync(prompt);
                    fullResponse.AppendLine(chunkResponse.ToString());
                    fullResponse.AppendLine();
                }

                var summaryPrompt = $"Please provide a concise summary of the following analyses, focusing on answering the question: {question}\n\n{fullResponse}";
                var finalResponse = await GetResponseAsync(summaryPrompt);

                return finalResponse.ToString();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
        private async Task<string> ExtractTextFromPdfAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var pdfReader = new PdfReader(stream);
            using var pdfDocument = new PdfDocument(pdfReader);
            var text = new StringBuilder();

            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
            {
                var page = pdfDocument.GetPage(i);
                var strategy = new SimpleTextExtractionStrategy();
                string pageText = PdfTextExtractor.GetTextFromPage(page, strategy);
                text.AppendLine(pageText);
            }

            return $"File: {file.FileName}\n\nContent:\n{text}\n\n";
        }

        private List<string> SplitIntoChunks(string text, int maxTokens = 4000)
        {
            var chunks = new List<string>();
            var currentChunk = new StringBuilder();
            var sentences = text.Split(new[] { ". ", "! ", "? " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var sentence in sentences)
            {
                if (currentChunk.Length + sentence.Length > maxTokens)
                {
                    chunks.Add(currentChunk.ToString());
                    currentChunk.Clear();
                }
                currentChunk.Append(sentence + ". ");
            }

            if (currentChunk.Length > 0)
            {
                chunks.Add(currentChunk.ToString());
            }

            return chunks;
        }



        public async Task<object> GetResponseAsync(string message)
        {
            _apiKey = _configuration.GetSection("ApiKeys:OpenAI").Value;
            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                return result.Choices[0].Message.Content;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error: Unable to get a response from OpenAI. Status Code: {response.StatusCode}. Content: {errorContent}");
            }
        }
    }


}
