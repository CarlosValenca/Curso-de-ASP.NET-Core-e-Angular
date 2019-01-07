// IMPORTANTE: Dentro de Eventos.IO.Domain eu cliquei em Dependencies
// e com o bot�o direito eu cliquei em Add Reference e l� dentro eu procurei
// por Solution e escolhi o Eventos.IO.Domain.Core, criando assim uma dependencia
// com este outro projeto e assim podemos herdar na classe Evento a Entity
// O link abaixo fala sobre a compatibilidade do dotnet core ou standard
// https://github.com/dotnet/standard/blob/master/docs/versions.md
using Eventos.IO.Domain.Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventos.IO.Domain.Models
{
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

            /* Esta foi a primeira forma de tratar erros nesta entidade, iremos fazer de uma
             * nova forma
            ErrosValidacao = new Dictionary<string, string>();

            // Exceptions s�o caras, depende do SO, este custo computacional deve ser levado em conta
            // Isto pode gerar problemas de performance, pois pode roubar mem�ria do servidor
            // O ideal � evitar as exceptions sempre que necess�rio
            if (nome.Length < 3)
                ErrosValidacao.Add("Nome", "O nome n�o pode possuir menos de 3 caracteres");
            // throw new ArgumentException("O nome do evento deve ter mais de 3 caracteres");

            if (gratuito && valor != 0)
                ErrosValidacao.Add("Valor", "N�o pode ter valor se gratuito");
            // throw new ArgumentException("N�o pode ter valor se gratuito");

            // Isto impede da entidade ser criada de forma errada, como � uma �nica
            // excess�o n�o ir� onerar tanto o sistema
            if (ErrosValidacao.Any())
                throw new Exception("A entidade possui " + ErrosValidacao.Count() + " Erros");
            */
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
            // Depois de validar tudo, retornamos se o objeto � v�lido ou n�o
            return ValidationResult.IsValid;
        }

        #region Valida��es
        private void Validar()
        {
            ValidarNome();
            ValidarValor();
            ValidarData();
            ValidarLocal();
            ValidarNomeEmpresa();
            // Valide esta inst�ncia inteira com os m�todos acima
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
            // Aqui testamos se a regra funciona, caso contr�rio o WithMessage
            // retorna uma mensagem de erro dizendo que a regra falhou
            if (!Gratuito)
                RuleFor(c => c.Valor)
                    .ExclusiveBetween(1, 50000)
                    .WithMessage("O valor precisa estar entre 1 e 50.000");

            if (Gratuito)
                RuleFor(c => c.Valor)
                    .ExclusiveBetween(0, 0).When(e => e.Gratuito)
                    .WithMessage("O valor n�o deve ser diferente de 0 para um evento gratuito");
        }

        private void ValidarData()
        {
            RuleFor(c => c.DataInicio)
                .GreaterThan(c => c.DataFim)
                .WithMessage("A data de in�cio deve ser maior que a data do final do evento");

            RuleFor(c => c.DataInicio)
                .LessThan(DateTime.Now)
                .WithMessage("A data de in�cio n�o deve ser menor que a data atual");
        }

        private void ValidarLocal()
        {
            if (Online)
                RuleFor(c => c.Endereco)
                    .Null().When(c => Online)
                    .WithMessage("O evento n�o deve possuir um endere�o se for online");

            if (!Online)
                RuleFor(c => c.Endereco)
                    .NotNull().When(c => c.Online == false)
                    .WithMessage("O evento deve possuir um endere�o");
        }

        private void ValidarNomeEmpresa()
        {
            RuleFor(c => c.NomeEmpresa)
                .NotEmpty().WithMessage("O nome do organizador precisa ser fornecido")
                .Length(2, 150).WithMessage("O nome do organizador precisa ter entre 2 e 150 caracteres");
        }

        #endregion

        // public Dictionary<string, string> ErrosValidacao { get; set; }

        /* Este m�todo foi transferido para a classe Entity em Domain.Core
         * de modo a podermos validar n�o somente um Evento mas qualquer tipo
         * de objeto
        public bool EhValido()
        {
            // Se n�o tiver nenhum erro
            // return !ErrosValidacao.Any();
        }
        */
    }

}
