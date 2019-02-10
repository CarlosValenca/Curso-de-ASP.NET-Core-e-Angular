using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Eventos.IO.Site.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        // Não consigo interceptar esta razor page AccessDenied para chamar a controller de erros minuto 45 vídeo 18
        public void OnGet()
        {
            RedirectToPage("Erros", "Erros", new { id = 403 });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("Erros", "Erros", new { id = 403 });
            return RedirectToAction("Erros", "Erro", new { id = 403 });
        }
    }
}

