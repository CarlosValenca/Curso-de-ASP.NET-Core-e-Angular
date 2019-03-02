using System;
using System.Threading;
using System.Threading.Tasks;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Events;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Handlers;
using Eventos.IO.Domain.Interfaces;
using MediatR;

namespace Eventos.IO.Domain.Eventos.Commands
{
    public class EventoCommandHandler : CommandHandler,
        INotificationHandler<RegistrarEventoCommand>,
        INotificationHandler<AtualizarEventoCommand>,
        INotificationHandler<ExcluirEventoCommand>,
        INotificationHandler<IncluirEnderecoEventoCommand>,
        INotificationHandler<AtualizarEnderecoEventoCommand>
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IUser _user;
        private readonly IMediatorHandler _mediator;

        public EventoCommandHandler(IEventoRepository eventoRepository,
                                    IUnitOfWork uow,
                                    INotificationHandler<DomainNotification> notifications,
                                    IUser user,
                                    IMediatorHandler mediator) : base(uow, mediator, notifications)
        {
            _eventoRepository = eventoRepository;
            _user = user;
            _mediator = mediator;
        }

        public Task Handle(RegistrarEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Endereco.Id, message.Endereco.Logradouro, message.Endereco.Numero, message.Endereco.Complemento, message.Endereco.Bairro, message.Endereco.CEP, message.Endereco.Cidade, message.Endereco.Estado, message.Endereco.EventoId.Value);

            var evento = Evento.EventoFactory.NovoEventoCompleto(message.Id, message.Nome, message.DescricaoCurta,
                message.DescricaoLonga, message.DataInicio, message.DataFim, message.Gratuito, message.Valor,
                message.Online, message.NomeEmpresa, message.OrganizadorId, endereco, message.CategoriaId);

            if (!EventoValido(evento)) return Task.CompletedTask;

            // TODO:
            // Validações de negócio!
            // Organizador pode registrar evento ?

            // Persistência
            _eventoRepository.Adicionar(evento);
            if(Commit())
            {
                // Notificar Processo Concluído!
                // CQRS : Separar a leitura da escrita
                _mediator.PublicarEvento(new EventoRegistradoEvent(evento.Id, evento.Nome, evento.DataInicio, evento.DataFim, evento.Gratuito, evento.Valor, evento.Online, evento.NomeEmpresa));
            }

            return Task.CompletedTask;
        }

        public Task Handle(AtualizarEventoCommand message, CancellationToken cancellationToken)
        {
            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            if (!EventoExistente(message.Id, message.MessageType)) return Task.CompletedTask;

            // To Do: Validar se o evento pertence a pessoa que está editando. Usuário logado é diferente do usuário criador do evento, portanto não posso alterar
            // Com o GetUserId vc garante pegar o usuário que está de fato conectado no momento
            if (eventoAtual.OrganizadorId != _user.GetUserId())
            {
                _mediator.PublicarEvento(new DomainNotification(message.MessageType, "Evento não pertence ao Organizador"));
                return Task.CompletedTask;
            }

            var evento = Evento.EventoFactory.NovoEventoCompleto(message.Id, message.Nome, message.DescricaoCurta,
                message.DescricaoLonga, message.DataInicio, message.DataFim, message.Gratuito, message.Valor,
                message.Online, message.NomeEmpresa, message.OrganizadorId, eventoAtual.Endereco, message.CategoriaId);

            if (!evento.Online && evento.Endereco == null)
            {
                _mediator.PublicarEvento(new DomainNotification(message.MessageType, "Não é possível atualizar um evento sem informar o endereço"));
                return Task.CompletedTask;
            }

            if (!EventoValido(evento)) return Task.CompletedTask;

            _eventoRepository.Atualizar(evento);

            if(Commit())
            {
                _mediator.PublicarEvento(new EventoAtualizadoEvent(evento.Id, evento.Nome, evento.DescricaoCurta, evento.DescricaoLonga, evento.DataInicio, evento.DataFim, evento.Gratuito, evento.Valor, evento.Online, evento.NomeEmpresa));
            }

            return Task.CompletedTask;
        }

        public Task Handle(ExcluirEventoCommand message, CancellationToken cancellationToken)
        {
            if (!EventoExistente(message.Id, message.MessageType)) return Task.CompletedTask;

            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            // To Do: Validar se o evento pertence a pessoa que está excluindo. Usuário logado é diferente do usuário criador do evento, portanto não posso alterar
            // Com o GetUserId vc garante pegar o usuário que está de fato conectado no momento
            if (eventoAtual.OrganizadorId != _user.GetUserId())
            {
                _mediator.PublicarEvento(new DomainNotification(message.MessageType, "Evento não pertence ao Organizador"));
                return Task.CompletedTask;
            }

            eventoAtual.ExcluirEvento();

            _eventoRepository.Atualizar(eventoAtual);

            if(Commit())
            {
                _mediator.PublicarEvento(new EventoExcluidoEvent(message.Id));
            }

            return Task.CompletedTask;
        }

        private bool EventoValido(Evento evento)
        {
            if (evento.EhValido()) return true;

            NotificarValidacoesErro(evento.ValidationResult);
            return false;
        }

        private bool EventoExistente(Guid id, string messageType)
        {
            var evento = _eventoRepository.ObterPorId(id);

            if (evento != null) return true;

            _mediator.PublicarEvento(new DomainNotification(messageType, "Evento não encontrado."));

            return false;
        }

        public Task Handle(IncluirEnderecoEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Id, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.CEP, message.Cidade, message.Estado, message.EventoId.Value);
            if (!endereco.EhValido())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return Task.CompletedTask;
            }

            var evento = _eventoRepository.ObterPorId(message.EventoId.Value);
            evento.TornarPresencial();

            _eventoRepository.Atualizar(evento);
            _eventoRepository.AdicionarEndereco(endereco);

            if (Commit())
            {
                _mediator.PublicarEvento(new EnderecoEventoAdicionadoEvent(endereco.Id, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.CEP, endereco.Cidade, endereco.Estado, endereco.EventoId.Value));
            }

            return Task.CompletedTask;
        }

        public Task Handle(AtualizarEnderecoEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Id, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.CEP, message.Cidade, message.Estado, message.EventoId.Value);
            if (!endereco.EhValido())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return Task.CompletedTask;
            }
            _eventoRepository.AtualizarEndereco(endereco);

            if (Commit())
            {
                _mediator.PublicarEvento(new EnderecoEventoAtualizadoEvent(endereco.Id, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.CEP, endereco.Cidade, endereco.Estado, endereco.EventoId.Value));
            }

            return Task.CompletedTask;
        }

    }
}
