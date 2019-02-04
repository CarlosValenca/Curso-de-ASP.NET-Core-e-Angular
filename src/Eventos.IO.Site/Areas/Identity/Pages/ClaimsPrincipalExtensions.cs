using System;
using System.Security.Claims;

namespace Eventos.IO.Site.Areas.Identity.Pages
{
    public static class ClaimsPrincipalExtensions
    {
        // ClaimsPrincipal representa o usuário conectado na aplicação, não tem relação com o Identity e sim com o AspNet
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if(principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }
    }
}
