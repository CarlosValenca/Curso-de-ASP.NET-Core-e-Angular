using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Eventos.IO.TestesAutomatizados.Config
{
    public class SeleniunHelper
    {
        // CD = Chrome Driver
        public static IWebDriver CD;

        // O singleton é para aproveitar a mesma janela do browser para todos os testes pois do contrário a cada teste abriria uma janela nova
        private static SeleniunHelper _instance;
        // Quanto tempo vc vai esperar o Selenium retornar para a tela alguma resposta
        public WebDriverWait Wait;

        // Vc poderia escrever um outro método para escolher o browser para que ele seja setado dinamicamente neste método SeleniunHelper por exemplo
        protected SeleniunHelper()
        {
            // Assim como estamos usando o Chrome vc pode instalar a DLL de outros browsers para fazer o teste neles também
            CD = new ChromeDriver(ConfigurationHelper.ChromeDrive);

            CD.Manage().Window.Maximize();

            // Esperará 30 segundos por um retorno da tela a cada operação
            Wait = new WebDriverWait(CD, TimeSpan.FromSeconds(30));
        }

        // Devolve uma instância do Selenium
        public static SeleniunHelper Instance()
        {
            // Caso não exista uma instância do Selenium aberta eu crio uma instancia, do contrário mantenho a mesma instância aberta
            return _instance ?? (_instance = new SeleniunHelper());
        }

        public string ObterUrl()
        {
            // E a url atual da tela !
            return CD.Url;
        }

        // Aqui eu posso determinar se existe uma Url com determinado conteudo !
        public bool ValidarConteudoUrl(string conteudo)
        {
            // Espere por um retorno até que eu consiga retornar uma url que contenha o valor guardado em conteudo
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains(conteudo));
        }

        // Este método será utilizado para navegar até uma Url determinada
        public string NavegarParaUrl(string url)
        {
            CD.Navigate().GoToUrl(url);
            return CD.Url;
        }

        // Utilizado para obter o link e clicar neste link
        public void ClicarLinkPorTexto(string linkText)
        {
            var link = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.LinkText(linkText)));
            link.Click();
        }

        // Utilizado para obter o botão e clicar nele
        public void ClicarBotaoPorId(string botaoId)
        {
            var botao = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(botaoId)));
            botao.Click();
        }

        public void PreencherTextBoxPorId(string idCampo, string valorCampo)
        {
            var campo = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(idCampo)));
            // Vai simular o preenchimento do text box letra a letra para simular o comportamento humano

            for (int i = 0; i < valorCampo.Length; i++)
            {
                campo.SendKeys(valorCampo.Substring(i,1));
                Thread.Sleep(50);
            }
        }

        public string ObterTextoElementoPorClasse(string className)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName(className))).Text;
        }

        public string ObterTextoElementoPorId(string id)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(id))).Text;
        }

        // Retorna todos os elementos de uma class name específica
        public IEnumerable<IWebElement> ObterListaPorClasse(string className)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(className)));
        }

        // O ITakesScreenshot.GetScreenshot tira um print da tela
        public void ObterScreenShot(string nome)
        {
            var screenshot = ((ITakesScreenshot)CD).GetScreenshot();
            SalvarScreenShot(screenshot, string.Format("{0}_" + nome + ".png", DateTime.Now.ToFileTime()));
        }

        // Salva na pasta um arquivo
        private static void SalvarScreenShot(Screenshot screenshot, string fileName)
        {
            screenshot.SaveAsFile(string.Format("{0}{1}", ConfigurationHelper.FolderPicture, fileName),
                ScreenshotImageFormat.Png);
        }

        // *************************************** Métodos Criados por Carlos ***************************************
        // Criado para situações do tipo em que é necessário navegar apara algum campo para ativar alguma mensagem
        // como por exemplo do campo senha que fica desabilitado quando as senhas divergem e o teste trava
        public void SelecionaCampoTexto(string idCampo)
        {
            var campo = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(idCampo)));
            campo.Click();
        }

        // Campos como o DatePicker não são encontráveis por id
        public void PreencherCampoData(string idCampo, string valorData)
        {
            var dateBox = CD.FindElement(By.XPath(idCampo));
            // var campo = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(idCampo)));
            dateBox.Click();

            for (int i = 0; i < valorData.Length; i++)
            {
                dateBox.SendKeys(valorData.Substring(i, 1));
                Thread.Sleep(65);
            }

        }

        // Selecionar um elemento de uma lista drop down (por exemplo a de Categorias)
        public void PreencherCampoDropDown(string idCampo, string valorCampo)
        {
            var campo = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(idCampo)));
            campo.Click();
            var selectElement = new SelectElement(campo);
            selectElement.SelectByValue(valorCampo);
        }
    }
}
