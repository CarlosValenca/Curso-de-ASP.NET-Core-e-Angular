using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Eventos.IO.Infra.Data.Repository
{
    public class EventoRepository : Repository<Evento>, IEventoRepository
    {
        public EventoRepository(EventosContext context) : base(context)
        {

        }

        // Estamos usando o DAPPER aqui por questões de performance ele é muito mais rápido que o Entity Framework
        public override IEnumerable<Evento> ObterTodos()
        {
            var sql = "SELECT * FROM EVENTOS E " +
                      "WHERE E.EXCLUIDO = 0" +
                      "ORDER BY E.DATAFIM DESC";

            return Db.Database.GetDbConnection().Query<Evento>(sql);
        }

        // A variável Db é herdada do Repository.cs
        public void AdicionarEndereco(Endereco endereco)
        {
            Db.Enderecos.Add(endereco);
        }

        public void AtualizarEndereco(Endereco endereco)
        {
            Db.Enderecos.Update(endereco);
        }

        public Endereco ObterEnderecoPorId(Guid id)
        {
            var sql = @"SELECT * FROM Enderecos E " +
                       "WHERE E.Id = @uid";

            var endereco = Db.Database.GetDbConnection().Query<Endereco>(sql, new { uid = id });

            return endereco.SingleOrDefault();

            // Substituído o Entity Framework pelo Dapper
            // return Db.Enderecos.Find(id);
        }

        public IEnumerable<Evento> ObterEventoPorOrganizador(Guid organizadorId)
        {
            var sql = @"SELECT * FROM EVENTOS E " +
                        "WHERE E.EXCLUIDO = 0 " +
                        "AND E.ORGANIZADORID = @oid " +
                        "ORDER BY E.DATAFIM DESC";

            return Db.Database.GetDbConnection().Query<Evento>(sql, new { oid = organizadorId });

            // Substituído o Entity Framework pelo Dapper
            // return Db.Eventos.Where(e => e.OrganizadorId == organizadorId);
        }

        public override Evento ObterPorId(Guid id)
        {
            var sql = @"SELECT * FROM Eventos E " +
                      "LEFT JOIN Enderecos EN " + // Só traz o endereço se o mesmo existir
                      "ON E.Id = EN.EventoId " +
                      "WHERE E.Id = @uid"; // uid é uma espécie de parâmetro (para evitar o sql injection

            var evento = Db.Database.GetDbConnection().Query<Evento, Endereco, Evento>(sql,
                (e, en) =>
                {
                    // Se o endereco (en) existir, ele será retornado junto com o evento
                    if (en != null)
                        e.AtribuirEndereco(en);

                    return e;
                }, new { uid = id });

            return evento.FirstOrDefault();

            // Substituído o Entity Framework pelo Dapper na pesquisa
            // return Db.Eventos.Include(e => e.Endereco).FirstOrDefault(e => e.Id == id);
        }

        public override void Remover(Guid id)
        {
            var evento = ObterPorId(id);
            evento.ExcluirEvento();
            Atualizar(evento);
        }
    }
}