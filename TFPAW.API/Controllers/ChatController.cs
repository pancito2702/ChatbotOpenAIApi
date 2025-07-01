using Microsoft.AspNetCore.Mvc;
using TFPAW.Services;

namespace TFPAW.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatBotServiceFactory _chatBotServiceFactory;
        private readonly IPdfChatService _pdfChatService;

        public ChatController(IChatBotServiceFactory chatBotServiceFactory, IPdfChatService pdfChatService)
        {
            _chatBotServiceFactory = chatBotServiceFactory;
            _pdfChatService = pdfChatService;
        }

    


        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(string message)
        {
            IChatBotStrategy chatbot = _chatBotServiceFactory.GetInstance("Chat");
            object response = await chatbot.GetResponseAsync(message);
            return Ok(new { response });
        }

        [HttpPost("SendMessageDallE")]
        public async Task<IActionResult> SendMessageDallE(string message)
        {
            IChatBotStrategy dallE = _chatBotServiceFactory.GetInstance("DallE");
            object response = await dallE.GetResponseAsync(message); ;
            return Ok(new { response });
        }

        [HttpPost("ProcessPdf")]
        public async Task<IActionResult> ProcessPdf(IFormFileCollection files, string question)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files uploaded");
            }

            var response = await _pdfChatService.ProcessPdfAndRespond(files, question);
            return Ok(new { response });
        }

    }

  
}
