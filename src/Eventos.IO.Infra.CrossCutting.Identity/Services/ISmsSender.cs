using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventos.IO.Infra.CrossCutting.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
