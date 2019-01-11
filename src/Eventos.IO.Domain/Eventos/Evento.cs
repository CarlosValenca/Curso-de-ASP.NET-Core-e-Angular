// IMPORTANTE: Dentro de Eventos.IO.Domain eu cliquei em Dependencies
// e com o botão direito eu cliquei em Add Reference e lá dentro eu procurei
// por Solution e escolhi o Eventos.IO.Domain.Core, criando assim uma dependencia
// com este outro projeto e assim podemos herdar na classe Evento a Entity
// O link abaixo fala sobre a compatibilidade do dotnet core ou standard
// https://github.com/dotnet/standard/blob/master/docs/versions.md
using Eventos.IO.Domain.Core.Models;
using Eventos.IO.Domain.Organizadores;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Eventos
{
    // O evento é a raiz de agregação de todas as outras entidades
    public class Evento : Entity<Evento>
    {
        public Evento(string nome,
                      DateTime dataInicio,
                      DateTime dataFim,
                      bool gratuito,
                      decimal valor,
                      bool online,
                      string nomeEmpresa)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Gratuito = gratuito;
            Valor = valor;
            Online = online;
            NomeEmpresa = nomeEmpresa;
        }

        private Evento()
        {

        }

        public string Nome { get; private set; }
        public string DescricaoCurta { get; private set; }
        public string DescricaoLonga { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public bool Gratuito { get; private set; }
        public decimal Valor { get; private set; }
        public bool Online { get; private set; }
        public string NomeEmpresa { get; private set; }
        public Categoria Categoria { get; private set; }
        public ICollection<Tags> Tags { get; private set; }
        public Endereco Endereco { get; private set; }
        public Organizador Organizador { get; private set; }

        public override bool EhValido()
        {
            Validar();
            // Depois de validar tudo, retornamos se o objeto é válido ou não
            return ValidationResult.IsValid;
        }

        #region Validações

        private void Validar()
        {
            ValidarNome();
            ValidarValor();
            ValidarData();
            ValidarLocal();
            ValidarNomeEmpresa();
            // Valide esta instância inteira com os métodos acima
            ValidationResult = Validate(this);
        }

        private void ValidarNome()
        {
            // Podemos substituir o texto por uma chave dentro das aspas duplas
            // Por exemplo eventNameEmpty, podemos utilizar um arquivo de resource para conter
            // estas mensagens
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome do evento precisa ser fornecido")
                .Length(2, 150).WithMessage("O nome do evento precisa ter entre 2 e 150 caracteres");

        }

        private void ValidarValor()
        {
            // Aqui testamos se a regra funciona, caso contrário o WithMessage
            // retorna uma mensagem de erro dizendo que a regra falhou
            if (!Gratuito)
                RuleFor(c => c.Valor)
                    .ExclusiveBetween(1, 50000)
                    .WithMessage("O valor precisa estar entre 1 e 50.000");

            if (Gratuito)
                RuleFor(c => c.Valor)
                    .Equal(0).When(e => e.Gratuito)
                    .WithMessage("O valor não deve ser diferente de 0 para um evento gratuito");
        }

        private void ValidarData()
        {
            RuleFor(c => c.DataInicio)
                .LessThan(c => c.DataFim)
                .WithMessage("A data de início deve ser maior que a data do final do evento");

            RuleFor(c => c.DataInicio)
                .GreaterThan(DateTime.Now)
                .WithMessage("A data de início não deve ser menor que a data atual");
        }

        private void ValidarLocal()
        {
            if (Online)
                RuleFor(c => c.Endereco)
                    .Null().When(c => Online)
                    .WithMessage("O evento não deve possuir um endereço se for online");


            if (!Online)
                RuleFor(c => c.Endereco)
                    .NotNull().When(c => c.Online == false)
                    .WithMessage("O evento deve possuir um endereço");
        }

        private void ValidarNomeEmpresa()
        {
            RuleFor(c => c.NomeEmpresa)
                .NotEmpty().WithMessage("O nome do organizador precisa ser fornecido")
                .Length(2, 150).WithMessage("O nome do organizador precisa ter entre 2 e 150 caracteres");
        }

        #endregion

        public static class EventoFactory
        {
            public static Evento NovoEventoCompleto(Guid id, string nome, string descCurta, string descLonga, DateTime dataInicio, DateTime dataFim, bool gratuito, decimal valor, bool online, string nomeEmpresa, Guid? organizadorId)
            {
                var evento = new Evento()
                {
                    Id = id,
                    Nome = nome,
                    DescricaoCurta = descCurta,
                    DescricaoLonga = descLonga,
                    DataInicio = dataInicio,
                    DataFim = dataFim,
                    Gratuito = gratuito,
                    Valor = valor,
                    Online = online,
                    NomeEmpresa = nomeEmpresa
                };

                if(organizadorId != null)
                    evento.Organizador = new Organizador(organizadorId.Value);

                return evento;

            }
        }

    }

}
