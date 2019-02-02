using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eventos.IO.Application.ViewModels;

namespace Eventos.IO.Site.Data
{
    public class ApplicationDbContext : IdentityDbContext
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
