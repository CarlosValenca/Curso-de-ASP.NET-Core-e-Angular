// Identity: Inclua aqui os novos campos do Identity, é necessário fazer uma migration se desejar guardar na tabela AspNetUsers
using Microsoft.AspNetCore.Identity;

namespace Eventos.IO.Site.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Nome { get; set; }

        [PersonalData]
        public string CPF { get; set; }
    }
}