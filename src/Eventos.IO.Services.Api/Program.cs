using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Eventos.IO.Services.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        // Foi necessário adicionar o ValidateScopes para que não desse erro ao usar o IUser
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false);
    }
}
