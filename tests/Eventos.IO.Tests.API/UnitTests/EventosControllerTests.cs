using AutoMapper;
using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Services.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

// TESTES UNITÁRIOS
// A idéia geral ao criar tudo isto é confirmar que pelo menos estas validações não quebraram após mudanças no meu código
// O ganho do tempo aqui é não perder tempo testando individualmente tudo de novo executando a aplicação, é uma garantia que o seu código está funcionando
// Não estamos validando que a aplicação em si está funcionando
namespace Eventos.IO.Tests.API.UnitTests
{
    public class EventosControllerTests
    {
        // AAA => Arrange, Act, Assert

        public EventosController eventosController;

        public Mock<DomainNotificationHandler> mockNotification;
        public Mock<IMapper> mockMapper;
        public Mock<IMediatorHandler> mockMediator;

        public EventosControllerTests()
        {
            // Injeções de dependência expostas
            mockNotification = new Mock<DomainNotificationHandler>();
            mockMapper = new Mock<IMapper>();
            mockMediator = new Mock<IMediatorHandler>();

            // Injeções de dependência não expostas
            var mockUser = new Mock<IUser>();
            var mockAppService = new Mock<IEventoAppService>();
            var mockRepository = new Mock<IEventoRepository>();

            eventosController = new EventosController(
            mockNotification.Object,
            mockUser.Object,
            mockMediator.Object,
            mockAppService.Object,
            mockRepository.Object,
            mockMapper.Object);
        }

        // Aqui estamos testando uma simulação da realidade, vamos testar se o método POST da controller sabe criar um novo evento
        [Fact]
        // Eu consigo criar um evento ?
        public void EventosController_RegistrarEvento_RetornarComSucesso()
        {
            // Arrange (Criar: Estou criando uma instância representativa dos objetos que estão com injeção de dependência)

            var eventoViewModel = new EventoViewModel();
            // Estou criando um evento fake para checar as regras de validação da criação de um evento
            var eventoCommand = new RegistrarEventoCommand("Teste", "", "", DateTime.Now, DateTime.Now.AddDays(1), true,
                0, true, "", Guid.NewGuid(), Guid.NewGuid(),
                new IncluirEnderecoEventoCommand(Guid.NewGuid(), "", null, "", "", "", "", "", null));

            // Estou configurando o Mapper para retornar um eventoCommand quando o comando Map for utilizado
            mockMapper.Setup(m => m.Map<RegistrarEventoCommand>(eventoViewModel)).Returns(eventoCommand);
            // Estou configurando o o componente de notificações para retornar uma lista vazia de notificações
            mockNotification.Setup(m => m.GetNotifications()).Returns(new List<DomainNotification>());

            // Act (Testar)
            var result = eventosController.Post(eventoViewModel);

            // Assert (Validar)
            mockMediator.Verify(m => m.EnviarComando(eventoCommand), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        /*
         * A idéia do próximo Fact é testar o trecho abaixo que está no Post desta Controller
         * if (!ModelState.IsValid)
         * {
         *     NotificarErroModelInvalida();
         * }
         */
        [Fact]
        // Meu código sabe validar se a minha view model está válida ?
        public void EventosController_RegistrarEvento_RetornarComErrosDeModelState()
        {
            // Arrange

            // Estou criando uma notificação de Erro de Model State
            var notificationList = new List<DomainNotification> { new DomainNotification("Erro", "ModelError") };

            // Estou colocando a notificação criada no resultado do método GetNotifications
            mockNotification.Setup(m => m.GetNotifications()).Returns(notificationList);
            // Estou dizendo que o método HasNotifications terá uma notificação
            mockNotification.Setup(c => c.HasNotifications()).Returns(true);
            
            // Estou informando o código de erro para Model State Inválida
            eventosController.ModelState.AddModelError("Erro", "ModelError");

            // Act
            var result = eventosController.Post(new EventoViewModel());

            // Assert (Validar) : Utilizamos o Never para garantir que por conta do código de erro nunca este comando será executado (isto tb acaba sendo um teste)
            // O It é uma classe do MOQ que vai representar um comando fake
            mockMediator.Verify(m => m.EnviarComando(It.IsAny<RegistrarEventoCommand>()), Times.Never);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        // Meu código sabe validar se houve algum erro no domínio ?
        public void EventosController_RegistrarEvento_RetornarComErrosDeDominio()
        {
            // Arrange

            var eventoViewModel = new EventoViewModel();

            var eventoCommand = new RegistrarEventoCommand("Teste", "", "", DateTime.Now, DateTime.Now.AddDays(1), true,
                0, true, "", Guid.NewGuid(), Guid.NewGuid(),
                new IncluirEnderecoEventoCommand(Guid.NewGuid(), "", null, "", "", "", "", "", null));

            mockMapper.Setup(m => m.Map<RegistrarEventoCommand>(eventoViewModel)).Returns(eventoCommand);

            // Estou criando uma notificação de Erro de Domínio
            var notificationList = new List<DomainNotification> { new DomainNotification("Erro", "Erro ao adicionar o evento") };

            mockNotification.Setup(m => m.GetNotifications()).Returns(notificationList);
            mockNotification.Setup(c => c.HasNotifications()).Returns(true);

            // Act
            var result = eventosController.Post(new EventoViewModel());

            // Assert (Validar) : Utilizamos o AtLeastOnce para garantir que o Mediator será executado uma vez que este comando vem antes das validações de domínio
            mockMediator.Verify(m => m.EnviarComando(It.IsAny<RegistrarEventoCommand>()), Times.AtLeastOnce);
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
