using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Eventos.IO.Services.Api.Middlewares
{
    public class SwaggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUser _user;

        public SwaggerMiddleware(RequestDelegate next, IUser user)
        {
            _next = next;
            _user = user;
        }

        public async Task Invoke(HttpContext context)
        {
            // Aqui temos uma proteção para acessar as apis do projeto dentro do swagger, vc necessariamente precisará
            // estar autenticado para poder ver e testar as apis
            if (context.Request.Path.StartsWithSegments("/swagger")
                && !_user.IsAuthenticated())
                {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
                }
            // Chama o próximo Middleware
            await _next.Invoke(context);
        }
    }

    public static class SwaggerMiddlewareExtension
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerMiddleware>();
        }
    }

}
