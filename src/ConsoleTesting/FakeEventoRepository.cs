using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Eventos.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

// Estas classes Fake existem para simular o domínio sem criar persistencia de dados visto que
// ainda não estamos nos conectando ao banco de dados

public class FakeEventoRepository : IEventoRepository
{
    public void Add(Evento obj)
    {
        // nada por enquanto...
    }

    public void Dispose()
    {
        // nada por enquanto...
    }

    public IEnumerable<Evento> Find(Expression<Func<Evento, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Evento> GetAll()
    {
        throw new NotImplementedException();
    }

    public Evento GetById(Guid id)
    {
        // Aqui estamos simulando a existência de um evento
        return new Evento("Fake", DateTime.Now, DateTime.Now, true, 0, true, "Empresa");

        // Aqui estamos simulando a inexistência de um evento, habilitar este trecho para testar isto
        // return null;
    }

    public void Remove(Guid id)
    {

    }

    public int SaveChanges()
    {
        throw new NotImplementedException();
    }

    public void Update(Evento obj)
    {
        // nada por enquanto...
    }
}