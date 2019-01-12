using System;

namespace Eventos.IO.Domain.Eventos.Commands
{
    public class RegistrarEventoCommand : BaseEventoCommand
    {
        public RegistrarEventoCommand(
                    string nome,
                    DateTime dataInicio,
                    DateTime dataFim,
                    bool gratuito,
                    decimal valor,
                    bool online,
                    string nomeEmpresa,
                    Guid organizadorId,
                    Guid categoriaId,
                    IncluirEnderecoEventoCommand endereco)
        {
            Nome = nome;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Gratuito = gratuito;
            Valor = valor;
            Online = online;
            NomeEmpresa = nomeEmpresa;
            OrganizadorId = organizadorId;
            CategoriaId = categoriaId;
            Endereco = endereco;
        }

        public IncluirEnderecoEventoCommand Endereco { get; private set; }

    }
}
