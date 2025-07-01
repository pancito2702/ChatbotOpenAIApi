using Microsoft.AspNetCore.Mvc;
using TFPAW.Services;
using System.Threading.Tasks;
using TFPAW.Models;
using TFPAW.Web.CoF;


namespace TFPAW.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IChatBotServiceFactory _chatBotServiceFactory;
        private readonly IPdfChatService _pdfChatService;

        public HomeController(IChatBotServiceFactory chatBotServiceFactory, IPdfChatService pdfChatService)
        {
            _chatBotServiceFactory = chatBotServiceFactory;
            _pdfChatService = pdfChatService;
           
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult FileReader()
        {
            return View();
        }

        public IActionResult DalleChat ()
        {
            return View();
        }
        public IActionResult ChatBot() {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {

            MessageHandler messageHandler = new MessageHandler();



            string errorMessage = messageHandler.Handle(message, null);

        
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Json(new { response = errorMessage });
            }

            IChatBotStrategy chatbot = _chatBotServiceFactory.GetInstance("Chat");
            object response = await chatbot.GetResponseAsync(message);
          
            return Json(new { response });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageDallE(string message)
        {
            MessageHandler messageHandler = new MessageHandler();
    

        
            string errorMessage = messageHandler.Handle(message, null);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Json(new { error = errorMessage });
            }
            // SOLID: Principio de Substitucion de Liskov
            // La clase base puede substituirse por una de sus subsclases

            IChatBotStrategy dallE = _chatBotServiceFactory.GetInstance("DallE");
            object response = await dallE.GetResponseAsync(message);
         
            return Json(new { response });
        }


        [HttpPost]
        public async Task<IActionResult> ProcessPdf(IFormFileCollection files, string question)
        {
            MessageHandler messageHandler = new MessageHandler();
            PdfHandler pdfHandler = new PdfHandler();
            PdfFormatValidatorHandle pdfFormatValidatorHandler = new PdfFormatValidatorHandle();

            messageHandler.SetNext(pdfHandler);
            pdfHandler.SetNext(pdfFormatValidatorHandler);
            string errorMessage = messageHandler.Handle(question, files);
            
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Json(new { response = errorMessage });
            }


            var response = await _pdfChatService.ProcessPdfAndRespond(files, question);
   
            return Json(new { response });
        }
    }
}
