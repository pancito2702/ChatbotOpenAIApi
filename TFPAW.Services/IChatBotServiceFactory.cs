using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFPAW.Services
{
    public interface IChatBotServiceFactory 
    {
        IChatBotStrategy GetInstance(string service);
    }
}
