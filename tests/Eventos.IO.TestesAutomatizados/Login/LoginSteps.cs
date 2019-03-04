using Eventos.IO.TestesAutomatizados.Config;

namespace Eventos.IO.TestesAutomatizados.Login
{
    public class LoginSteps
    {
        public static void Login(SeleniunHelper browser)
        {
            browser.ClicarLinkPorTexto("Entrar");
            browser.PreencherTextBoxPorId("email", ConfigurationHelper.TestUserName);
            browser.PreencherTextBoxPorId("senha", ConfigurationHelper.TestPassword);
            browser.ClicarBotaoPorId("Login");
        }
    }
}
