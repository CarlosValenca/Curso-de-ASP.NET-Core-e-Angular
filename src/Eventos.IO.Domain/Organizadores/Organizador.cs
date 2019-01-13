// IMPORTANTE: Dentro de Eventos.IO.Domain eu cliquei em Dependencies
// e com o botão direito eu cliquei em Add Reference e lá dentro eu procurei
// por Solution e escolhi o Eventos.IO.Domain.Core, criando assim uma dependencia
// com este outro projeto e assim podemos herdar na classe Evento a Entity
using System;
using System.Collections.Generic;
using Eventos.IO.Domain.Core.Models;
using Eventos.IO.Domain.Eventos;

namespace Eventos.IO.Domain.Organizadores
{
    public class Organizador : Entity<Organizador>
    {
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }

        public Organizador(Guid id, string nome, string cpf, string email)
        {
            Id = id;
            Nome = nome;
            CPF = cpf;
            Email = email;
        }

        // EF Construtor
        protected Organizador() { }

        // EF Propriedade de Navegação : um organizador pode ter muitos eventos
        public virtual ICollection<Evento> Eventos { get; set; }

        public override bool EhValido()
        {
            return true;
        }
    }
}