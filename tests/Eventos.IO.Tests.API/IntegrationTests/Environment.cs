using Eventos.IO.Services.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace Eventos.IO.Tests.API.IntegrationTests
{
    // Configurar meu servidor
    public class Environment
    {
        // Representa o servidor
        public static TestServer Server;
        // Representa o cliente (não é o Angular ou MVC é como se fosse um deles)
        public static HttpClient Client;

        public static void CriarServidor()
        {
            Server = new TestServer(
                new WebHostBuilder()
                // Ajuda a escolher o appsettings.json conforme o ambiente
                .UseEnvironment("Testing")
                .UseUrls("http://localhost:8285")
                .UseStartup<StartupTests>());

            Client = Server.CreateClient();
        }
    }
}