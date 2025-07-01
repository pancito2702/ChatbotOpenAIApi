using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFPAW.Services
{
    public class ChatBotServiceFactory : IChatBotServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public ChatBotServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IChatBotStrategy GetInstance(string service)
        {
            switch (service)
            {
                case "Chat":
                    return _serviceProvider.GetRequiredService<ChatService>();
                case "DallE":
                    return _serviceProvider.GetRequiredService<DallEService>();
                default:
                    throw new ArgumentException("Invalid service type");
            }
        }
    }
}
