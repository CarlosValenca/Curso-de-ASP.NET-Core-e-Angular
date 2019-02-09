using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eventos.IO.Infra.CrossCutting.Data
{
    // Identity: Informado o ApplicationUser dentro do IdentityDbContext conforme auxiliado pelo Marmoré
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Usado para o EF
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Colocado apenas para a criação da controller, apos a geração não e mais necessario
        // public DbSet<Eventos.IO.Application.ViewModels.EventoViewModel> EventoViewModel { get; set; }
    }
}
