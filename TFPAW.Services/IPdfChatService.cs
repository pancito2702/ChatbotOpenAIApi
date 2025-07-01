using Microsoft.AspNetCore.Http;

namespace TFPAW.Services
{
    public interface IPdfChatService : IChatBotStrategy
    {
        Task<string> ProcessPdfAndRespond(IFormFileCollection files, string question);
    }
}