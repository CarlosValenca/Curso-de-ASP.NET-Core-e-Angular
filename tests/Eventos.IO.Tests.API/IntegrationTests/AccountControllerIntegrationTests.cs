using Eventos.IO.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Eventos.IO.Tests.API.IntegrationTests.DTO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

// IMPORTANTE: Para que o arquivo appsettings.testing.json (ou qualquer outro arquivo que vc criar novo aparecer na pasta
// Eventos.IO\tests\Eventos.IO.Tests.API\bin\Debug\netcoreapp2.2), é necessário dar um rebuild da solução inteira !
namespace Eventos.IO.Tests.API.IntegrationTests
{
    public class AccountControllerIntegrationTests
    {
        public AccountControllerIntegrationTests()
        {
            Environment.CriarServidor();
        }

        [Fact]
        public async Task AccountController_RegistrarOrganizador_RetornarComSucesso()
        {
            // Arrange: não esqueça de apagar o organizador/usuario abaixo
            var user = new RegisterViewModel
            {
                Nome = "Cliente Debug de Teste",
                CPF = "99999999999",
                Email = "clientedebugdeteste@me.com",
                Senha = "Temp@123",
                SenhaConfirmacao = "Temp@123"
            };

            // Para o SerializeObject ser reconhecido precisei instalar o pacote Json.Net
            var postContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act : Simulando um response em Json como se fosse o cliente fazendo
            var response = await Environment.Client.PostAsync("/api/vi/nova-conta", postContent);

            // Esta variável meuObjeto existe apenas para auxiliar na criação dos métodos da minha DTO e por isto está comentado para fins de referência
            // Desejando olhar o conteúdo e regerar a DTO basta tirar o comentário
            // var meuObjeto = await response.Content.ReadAsStringAsync();

            var userResult = JsonConvert.DeserializeObject<UsuarioJsonDTO>(await response.Content.ReadAsStringAsync());

            // Assert: Aqui estou garantindo que meu retorno é 200 (OK), que existe um token e que o mesmo não está vazio
            response.EnsureSuccessStatusCode();
            var token = userResult.data.access_token;
            Assert.NotEmpty(token);
        }
    }
}
