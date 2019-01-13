/*
 * INSTRUÇÕES para fazer a Migration Corretamente
 * 1) Criar o projeto Eventos.IO.Site .NetCore 2.2 Console Application com Identity,
 * configurar ele como projeto inicial, no appsettings mudar a string do banco conforme o appsettings
 * do Eventos.IO.Infra.Data
 * 2) Na Package Manager Console e com o "Default Project - 5 - Infra\5.1 - Data\Eventos.IO.Infra.Data"
 * escolhido executar os seguintes comandos
 * 2.1) add-migration -Context EventosContext Initial
 * 2.2) update-database -Context EventosContext
 * 2.3) confirmar no sql-server local que as tabelas de Eventos foram criadas no banco EventosIODemo
 * 3) Na Package Manager Console e com o "Default Project - 1 - Presentation\Eventos.IO.Site"
 * escolhido executar os seguintes comandos
 * 3.1) add-migration -Context ApplicationDbContext Initial
 * 3.2) update-database -Context ApplicationDbContext
 * 3.3) confirmar no sql-server local que as tabelas do Identity foram criadas no banco EventosIODemo
 * 4) Depois de fazer os Mappings:
 * 4.1) add-migration Tabelas-ComCamposTamanhoCorreto -Context EventosContext
 * 4.2) update-database -Context EventosContext
 * 4.3) confirmar no sql-server local que as tabelas de Eventos foram criadas no banco EventosIODemo
*/

using System.IO;
using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Organizadores;
using Eventos.IO.Infra.Data.Extensions;
using Eventos.IO.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Eventos.IO.Infra.Data.Context
{
    public class EventosContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Organizador> Organizadores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new EventoMapping());
            modelBuilder.AddConfiguration(new OrganizadorMapping());
            modelBuilder.AddConfiguration(new EnderecoMapping());
            modelBuilder.AddConfiguration(new CategoriaMapping());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Estou pedindo para usar o appsettings no base path que é o arquivo do projeto
            // no appsettings eu informei o nome do banco de dados local que eu quero que a
            // migration crie. Uma outra informação importante: informar a propriedade 
            // Copy to Output Directory como Copy Always para jogar este arquivo json
            // na pasta bin de modo a gerar o banco de dados corretamente
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}