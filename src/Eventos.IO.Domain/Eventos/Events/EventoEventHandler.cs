using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System;

namespace Eventos.IO.Domain.Eventos.Events
{
    public class EventoEventHandler :
        INotificationHandler<EventoRegistradoEvent>,
        INotificationHandler<EventoAtualizadoEvent>,
        INotificationHandler<EventoExcluidoEvent>,
        INotificationHandler<EnderecoEventoAdicionadoEvent>,
        INotificationHandler<EnderecoEventoAtualizadoEvent>

    {
        public Task Handle(EventoRegistradoEvent message, CancellationToken cancellationToken)
        {
            // Enviar um email
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Evento Registrado com Sucesso");
            return Task.CompletedTask;
        }

        public Task Handle(EventoAtualizadoEvent message, CancellationToken cancellationToken)
        {
            // Enviar um email
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Evento Atualizado com Sucesso");
            return Task.CompletedTask;
        }

        public Task Handle(EventoExcluidoEvent message, CancellationToken cancellationToken)
        {
            // Enviar um email
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Evento Excluído com Sucesso");
            return Task.CompletedTask;
        }

        public Task Handle(EnderecoEventoAdicionadoEvent message, CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereço do Evento Adicionado com Sucesso");
            return Task.CompletedTask;
        }

        public Task Handle(EnderecoEventoAtualizadoEvent message, CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereço do Evento Atualizado com Sucesso");
            return Task.CompletedTask;
        }
    }
}
