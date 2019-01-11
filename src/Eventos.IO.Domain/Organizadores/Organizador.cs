// IMPORTANTE: Dentro de Eventos.IO.Domain eu cliquei em Dependencies
// e com o botão direito eu cliquei em Add Reference e lá dentro eu procurei
// por Solution e escolhi o Eventos.IO.Domain.Core, criando assim uma dependencia
// com este outro projeto e assim podemos herdar na classe Evento a Entity
using Eventos.IO.Domain.Core.Models;
using System;

namespace Eventos.IO.Domain.Organizadores
{
    public class Organizador : Entity<Organizador>
    {
        public Organizador(Guid id)
        {
            Id = id;
        }

        public override bool EhValido()
        {
            throw new System.NotImplementedException();
        }
    }
}