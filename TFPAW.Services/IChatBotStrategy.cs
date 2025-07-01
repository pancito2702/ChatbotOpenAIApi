using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFPAW.Services
{
    // DESIGN PATTERN: STRATEGY 
    // SOLID: Principio de Segragacion de Interfaz, solo las clases que necesitan obtener una respuesta asincrona
    // implementan esta interfaz
    public interface IChatBotStrategy
    {
        Task<object> GetResponseAsync(string message);

    }
}
