using System;

namespace Eventos.IO.Tests.API.IntegrationTests.DTO
{
    /* Para ter o conteúdo abaixo:
     * 1) Excluir o Organizador/Usuário "Cliente Debug de Teste" (tem script sql na pasta tests)
     * 2) Realizar o debug de AccountController_RegistrarOrganizador_RetornarComSucesso com um breakpoint
     *    logo abaixo de var meuObjeto...
     * 3) Copiar o texto da variável meuObjeto
     * 4) Escolher "Edit", "Paste Special", "Paste Json as Classes"
     * 5) Substitua o nome da classe Rootobject por UsuarioJsonDTO
     * 6) Exclua a classe padrão previamente criada
     * 7) E então vc terá o conteúdo no formato abaixo
     */

    public class UsuarioJsonDTO
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string access_token { get; set; }
        public DateTime expires_in { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public Claim[] claims { get; set; }
    }

    public class Claim
    {
        public string type { get; set; }
        public string value { get; set; }
    }

}