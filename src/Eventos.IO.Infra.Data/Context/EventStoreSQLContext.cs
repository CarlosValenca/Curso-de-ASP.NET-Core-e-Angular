using System;
using System.IO;
using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Infra.Data.Extensions;
using Eventos.IO.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Eventos.IO.Infra.Data.Context
{
    public class EventStoreSQLContext : DbContext
    {
        public DbSet<StoredEvent> StoredEvent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new StoredEventMap());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // Obs2: Troquei o Directory.GetCurrentDirectory() por AppDomain.CurrentDomain.BaseDirectory
            // desta forma de fato pegamos o endereço onde está o appsettings correto para informar
            // a string de conexão
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            // As variáveis a seguir existem apenas para debugar a string de conexão
            var a = AppDomain.CurrentDomain.BaseDirectory;
            var b = config.GetConnectionString("SqlServerConnection");

            // Aqui podemos usar o "DefaultConnection" para trabalhar com o LocalDB ou o "SqlServerConnection" para trabalhar com o Sql Server 2017 instalado
            optionsBuilder.UseSqlServer(config.GetConnectionString("SqlServerConnection"));
        }
    }
}