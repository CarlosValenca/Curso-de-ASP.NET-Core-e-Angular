// Identity: Inclua aqui os novos campos do Identity, é necessário fazer uma migration se desejar guardar na tabela AspNetUsers
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventos.IO.Infra.CrossCutting.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Identity: NotMapped não tentará incluir o Nome nas tabelas do Identity conforme explicado pelo Marmoré
        [NotMapped]
        [PersonalData]
        public string Nome { get; set; }

        // Identity: NotMapped não tentará incluir o CPF nas tabelas do Identity conforme explicado pelo Marmoré
        [PersonalData]
        [NotMapped]
        public string CPF { get; set; }

    }
}