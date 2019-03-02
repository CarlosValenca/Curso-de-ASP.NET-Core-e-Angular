using AutoMapper;
using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Application.Services
{
    public class EventoAppService : IEventoAppService
    {
        private readonly IMediatorHandler _mediator;
        private readonly IMapper _mapper;
        private readonly IEventoRepository _eventoRepository;
        private readonly IUser _user;

        // O bus vai enviar estas mensagens para uma fila
        public EventoAppService(IMediatorHandler mediator,
                                IMapper mapper,
                                IEventoRepository eventoRepository,
                                IUser user)
        {
            _mediator = mediator;
            _mapper = mapper;
            _eventoRepository = eventoRepository;
            _user = user;
        }

        public void Registrar(EventoViewModel eventoViewModel)
        {
            var registroCommand = _mapper.Map<RegistrarEventoCommand>(eventoViewModel);
            _mediator.EnviarComando(registroCommand);
        }

        public IEnumerable<EventoViewModel> ObterEventoPorOrganizador(Guid organizadorId)
        {
            return _mapper.Map<IEnumerable<EventoViewModel>>(_eventoRepository.ObterEventoPorOrganizador(organizadorId));
        }

        public EventoViewModel ObterPorId(Guid id)
        {
            /* Esta implementação é uma idéia de como poderiamos fazer uma implementação considerando o usuário logado
             * Neste caso em específico não vamos fazer assim pois o usuário também poderá usar este método no botão
             * detalhes, e no atualizar e excluir já estamos validando o organizador do evento pelo usuário logado
            var evento = _eventoRepository.ObterPorId(id);

            if(evento.OrganizadorId != _user.GetUserId())
            {

            }
            */

            return _mapper.Map<EventoViewModel>(_eventoRepository.ObterPorId(id));
        }

        public IEnumerable<EventoViewModel> ObterTodos()
        {
            return _mapper.Map<IEnumerable<EventoViewModel>>(_eventoRepository.ObterTodos());
        }

        public void Atualizar(EventoViewModel eventoViewModel)
        {
            var atualizarEventoCommmand = _mapper.Map<AtualizarEventoCommand>(eventoViewModel);
            _mediator.EnviarComando(atualizarEventoCommmand);
        }

        public void Excluir(Guid id)
        {
            _mediator.EnviarComando(new ExcluirEventoCommand(id));
        }

        public void AdicionarEndereco(EnderecoViewModel enderecoViewModel)
        {
            var enderecoCommand = _mapper.Map<IncluirEnderecoEventoCommand>(enderecoViewModel);
            _mediator.EnviarComando(enderecoCommand);
        }

        public void AtualizarEndereco(EnderecoViewModel enderecoViewModel)
        {
            var enderecoCommand = _mapper.Map<AtualizarEnderecoEventoCommand>(enderecoViewModel);
            _mediator.EnviarComando(enderecoCommand);
        }

        public EnderecoViewModel ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<EnderecoViewModel>(_eventoRepository.ObterEnderecoPorId(id));
        }

        public void Dispose()
        {
            _eventoRepository.Dispose();
        }

    }
}
