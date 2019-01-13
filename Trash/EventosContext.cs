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
            #region FluentAPI

            #region Evento
            modelBuilder.Entity<Evento>()
                .Property(e => e.Nome)
                .HasColumnType("varchar(150)")
                .IsRequired();

            modelBuilder.Entity<Evento>()
                .Property(e => e.DescricaoCurta)
                .HasColumnType("varchar(150)");

            modelBuilder.Entity<Evento>()
                .Property(e => e.DescricaoLonga)
                .HasColumnType("varchar(max)");

            modelBuilder.Entity<Evento>()
                .Property(e => e.NomeEmpresa)
                .HasColumnType("varchar(150)")
                .IsRequired();

            // Coisas para ignorar na hora de fazer o migration
            
            // Isto é só uma propriedade do Fluent Validation
            modelBuilder.Entity<Evento>()
                .Ignore(e => e.ValidationResult);

            modelBuilder.Entity<Evento>()
                .Ignore(e => e.Tags);

            // Isto tb é uma propriedade do Fluent Validation
            modelBuilder.Entity<Evento>()
                .Ignore(e => e.CascadeMode);

            // Para não correr o risco da tabela ficar "Eventoes" como se fosse inglês
            modelBuilder.Entity<Evento>()
                .ToTable("Eventos");

            // Aqui eu faço o meu relacionamento, estou dizendo que o evento possui um organizador
            // Também vou dizer que o organizador possui muitos eventos (fk)
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Organizador)
                .WithMany(o => o.Eventos)
                .HasForeignKey(e => e.OrganizadorId);
            
            // A categoria não é requerida !
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Categoria)
                .WithMany(e => e.Eventos)
                .HasForeignKey(e => e.CategoriaId)
                .IsRequired(false);

            #endregion

            #region Endereco

            // O endereço só está contido em um único evento (1:1)
            // FK entre Endereço e Evento
            // Não é requerido (um evento não precisa de um evento)
            modelBuilder.Entity<Endereco>()
                .HasOne(c => c.Evento)
                .WithOne(c => c.Endereco)
                .HasForeignKey<Endereco>(c => c.EventoId)
                .IsRequired(false);

            modelBuilder.Entity<Endereco>()
                .Ignore(c => c.ValidationResult);

            modelBuilder.Entity<Endereco>()
                .Ignore(c => c.CascadeMode);

            modelBuilder.Entity<Endereco>()
                .ToTable("Enderecos");

            #endregion

            #region Organizador

            modelBuilder.Entity<Organizador>()
                .Ignore(e => e.ValidationResult);

            modelBuilder.Entity<Organizador>()
                .Ignore(e => e.CascadeMode);

            modelBuilder.Entity<Organizador>()
                .ToTable("Organizadores");

            #endregion

            #region Categoria

            modelBuilder.Entity<Categoria>()
                .Ignore(c => c.ValidationResult);

            modelBuilder.Entity<Categoria>()
                .Ignore(c => c.CascadeMode);

            modelBuilder.Entity<Categoria>()
                .ToTable("Categorias");

            #endregion

            #endregion

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