// IMPORTANTE: Dentro de Eventos.IO.Domain eu cliquei em Dependencies
// e com o botão direito eu cliquei em Add Reference e lá dentro eu procurei
// por Solution e escolhi o Eventos.IO.Domain.Core, criando assim uma dependencia
// com este outro projeto e assim podemos herdar na classe Evento a Entity
using Eventos.IO.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Eventos
{
    public class Categoria : Entity<Categoria>
    {
        // O modelo de negócios preve 3 categorias, por isto não estamos incluindo nome, CRUD etc...
        public Categoria(Guid id)
        {
            Id = id;
        }

        public string Nome { get; private set; }

        // EF Propriedade de Navegação (uma categoria pode estar em vários eventos)
        public virtual ICollection<Evento> Eventos { get; set; }

        // Construtor para o EF
        protected Categoria() { }

        // A princípio entenderemos que está válido pois não teremos um CRUD para ela
        public override bool EhValido()
        {
            return true;
        }
    }
}