using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Handlers;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Domain.Organizadores.Events;
using Eventos.IO.Domain.Organizadores.Repository;
using MediatR;
// using Eventos.IO.Domain.CommandHandlers;
// using Eventos.IO.Domain.Core.Events;

namespace Eventos.IO.Domain.Organizadores.Commands
{
    public class OrganizadorCommandHandler : CommandHandler,
        INotificationHandler<RegistrarOrganizadorCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IOrganizadorRepository _organizadorRepository;

        public OrganizadorCommandHandler(
            IUnitOfWork uow,
            INotificationHandler<DomainNotification> notifications,
            IOrganizadorRepository organizadorRepository, IMediatorHandler mediator) : base(uow, mediator, notifications)
        {
            _organizadorRepository = organizadorRepository;
            _mediator = mediator;
        }

        public Task Handle(RegistrarOrganizadorCommand message, CancellationToken cancellationToken)
        {
            var organizador = new Organizador(message.Id, message.Nome, message.CPF, message.Email);

            if(!organizador.EhValido())
            {
                NotificarValidacoesErro(organizador.ValidationResult);
                return Task.CompletedTask;
            }

            // ToDo: Validar CPF e email Duplicados
            var organizadorExistente = _organizadorRepository
                .Buscar(o => o.CPF == organizador.CPF || o.Email == organizador.Email);

            if (organizadorExistente.Any())
            {
                _mediator.PublicarEvento(new DomainNotification(message.MessageType, "CPF ou e-mail já utilizados"));
            }

            // ToDo: add no repositório
            _organizadorRepository.Adicionar(organizador);

            if(Commit())
            {
                _mediator.PublicarEvento(new OrganizadorRegistradoEvent(organizador.Id, organizador.Nome, organizador.CPF, organizador.Email));
            }

            return Task.CompletedTask;
        }
    }
}
