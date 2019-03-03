using Eventos.IO.Application.ViewModels;
using Eventos.IO.Tests.API.IntegrationTests.DTO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

// IMPORTANTE: Para que o arquivo appsettings.testing.json (ou qualquer outro arquivo que vc criar novo aparecer na pasta
// Eventos.IO\tests\Eventos.IO.Tests.API\bin\Debug\netcoreapp2.2), é necessário dar um rebuild da solução inteira !
namespace Eventos.IO.Tests.API.IntegrationTests
{
    public class EventosControllerIntegrationTests
    {
        public EventosControllerIntegrationTests()
        {
            Environment.CriarServidor();
        }

        [Fact]
        public async Task EventosController_RegistrarNovoEvento_RetornarComSucesso()
        {
            // Arrange
            var login = await UserUtils.RealizarLoginOrganizador(Environment.Client);

            var evento = new EventoViewModel
            {
                Nome = "DevExperience",
                DescricaoCurta = "Um evento de tecnologia",
                DescricaoLonga = "Um evento de tecnologia",
                CategoriaId = new Guid("ac381ba8-c187-482c-a5cb-899ad7176137"),
                DataInicio = DateTime.Now.AddDays(1),
                DataFim = DateTime.Now.AddDays(2),
                Gratuito = false,
                Valor = 500,
                NomeEmpresa = "DevX",
                Online = true,
                Endereco = new EnderecoViewModel(),
                OrganizadorId = new Guid(login.user.id)
            };

            // Act
            var response = await Environment.Server
                .CreateRequest("api/v1/eventos")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(
                    request => request.Content = new StringContent(JsonConvert.SerializeObject(evento), Encoding.UTF8, "application/json"))
                .PostAsync();

            /*
             * Para fazer um PUT vc pode fazer algo assim
             * .And(request => request.Method = HttpMethod.Put)
             * .PostAsync(); -- Aqui o verbo Post será interpretado como Put por conta do comando acima
             * Caso vc queira um delete basta trocar o HttpMethod.Delete, e o PostAsync será interpretado como o verbo Delete
             */

            // Esta variável meuObjeto existe apenas para auxiliar na criação dos métodos da minha DTO e por isto está comentado para fins de referência
            // Desejando olhar o conteúdo e regerar a DTO basta tirar o comentário
            // var meuObjeto = await response.Content.ReadAsStringAsync();

            var eventoResult = JsonConvert.DeserializeObject<EventoJsonDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            
            // O status code é 200 (OK) ?
            response.EnsureSuccessStatusCode();
            // O resultado recebido é do tipo Evento ?
            Assert.IsType<EventoDTO>(eventoResult.data);
        }

        [Fact]
        public async Task EventosController_ObterListaEventos_RetornarJsonComSucesso()
        {
            // Arrange & Act
            var response = await Environment.Client.GetAsync("api/v1/eventos");
            var responseEvento = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(responseEvento);
        }

        [Fact]
        public async Task EventosController_ObterListaMeusEventos_RetornarJsonComSucesso()
        {
            // Arrange
            var user = await UserUtils.RealizarLoginOrganizador(Environment.Client);

            // Act
            var response = await Environment.Server
                .CreateRequest("api/v1/eventos/meus-eventos")
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Authorization", "Bearer " + user.access_token)
                .GetAsync();

            var responseEvento = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(responseEvento);
        }

    }
}
