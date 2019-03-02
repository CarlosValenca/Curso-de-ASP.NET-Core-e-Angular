using System;
using System.Collections.Generic;
using AutoMapper;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eventos.IO.Application.Interfaces;
//using Eventos.IO.Domain.Interfaces;

namespace Eventos.IO.Services.Api.Controllers
{
    public class EventosController : BaseController
    {

        private readonly IEventoAppService _eventoAppService;
        private readonly IMediatorHandler _mediator;
        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;

        public EventosController(
                                 INotificationHandler<DomainNotification> notifications,
                                 IUser user,
                                 IMediatorHandler mediator,
                                 IEventoAppService eventoAppService,
                                 IEventoRepository eventoRepository,
                                 IMapper mapper) : base(notifications, user, mediator)
        {
            _eventoAppService = eventoAppService;
            _eventoRepository = eventoRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("eventos")]
        [AllowAnonymous]
        public IEnumerable<EventoViewModel> Get()
        {
            return _eventoAppService.ObterTodos();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("eventos/{id:guid}")]
        public EventoViewModel Get(Guid id, int version)
        {
            return _eventoAppService.ObterPorId(id);
        }

        [HttpGet]
        [Authorize(Policy = "PodeLerEventos")]
        [Route("eventos/meus-eventos")]
        public IEnumerable<EventoViewModel> ObterMeusEventos()
        {
            return _mapper.Map<IEnumerable<EventoViewModel>>(_eventoRepository.ObterEventoPorOrganizador(OrganizadorId));
        }

        [HttpGet]
        [Authorize(Policy = "PodeLerEventos")]
        [Route("eventos/meus-eventos/{id:guid}")]
        public IActionResult ObterMeuEventoPorId(Guid id)
        {
            var evento = _mapper.Map<EventoViewModel>(_eventoRepository.ObterMeuEventoPorId(id, OrganizadorId));
            return evento == null ? StatusCode(404) : Response(evento);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("eventos/categorias")]
        public IEnumerable<CategoriaViewModel> ObterCategorias()
        {
            return _mapper.Map<IEnumerable<CategoriaViewModel>>(_eventoRepository.ObterCategorias());
        }

        [HttpPost]
        [Route("eventos")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Post([FromBody]EventoViewModel eventoViewModel)
        {
            // ssbcvp - voltar aqui - não está caindo exatamente aqui, está caindo em alguma validação genérica de erro no Postman que retorna o "title": "One or more validation errors occurred." com o response 400
            if (!ModelState.IsValid)
            {
                NotificarErroModelInvalida();
                return Response();
            }

            var eventoCommand = _mapper.Map<RegistrarEventoCommand>(eventoViewModel);

            _mediator.EnviarComando(eventoCommand);
            return Response(eventoCommand);
        }

        [HttpPut]
        [Route("eventos")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Put([FromBody]EventoViewModel eventoViewModel)
        {

            if (!ModelState.IsValid)
            {
                NotificarErroModelInvalida();
                return Response();
            }

            _eventoAppService.Atualizar(eventoViewModel);
            return Response(eventoViewModel);
        }

        [HttpDelete]
        [Route("eventos/{id:guid}")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Delete(Guid id)
        {
            _eventoAppService.Excluir(id);
            return Response();
        }


        [HttpPost]
        [Route("endereco")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Post([FromBody]EnderecoViewModel enderecoViewModel)
        {
            if (!ModelStateValida())
            {
                return Response();
            }

            var eventoCommand = _mapper.Map<IncluirEnderecoEventoCommand>(enderecoViewModel);

            _mediator.EnviarComando(eventoCommand);
            return Response(eventoCommand);
        }

        [HttpPut]
        [Route("endereco")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Put([FromBody]EnderecoViewModel enderecoViewModel)
        {
            if (!ModelStateValida())
            {
                return Response();
            }

            var eventoCommand = _mapper.Map<AtualizarEnderecoEventoCommand>(enderecoViewModel);

            _mediator.EnviarComando(eventoCommand);
            return Response(eventoCommand);
        }

        private bool ModelStateValida()
        {
            if (ModelState.IsValid) return true;

            NotificarErroModelInvalida();
            return false;
        }

    }
}
