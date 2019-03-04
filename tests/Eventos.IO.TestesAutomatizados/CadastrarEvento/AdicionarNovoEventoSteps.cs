using Eventos.IO.TestesAutomatizados.Config;
using System;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;

namespace Eventos.IO.TestesAutomatizados.CadastrarEvento
{
    [Binding]
    public class AdicionarNovoEventoSteps
    {

        public SeleniunHelper Browser;

        public AdicionarNovoEventoSteps()
        {
            Browser = SeleniunHelper.Instance();
        }

        [Given(@"que o Organizador está no site")]
        public void DadoQueOOrganizadorEstaNoSite()
        {
            var url = Browser.NavegarParaUrl(ConfigurationHelper.SiteUrl);
            Assert.Equal(ConfigurationHelper.SiteUrl, url);
        }

        [Given(@"Realiza o Login")]
        public void DadoRealizaOLogin()
        {
            Thread.Sleep(1000);
            Login.LoginSteps.Login(Browser);
            // Aguardará 18 segundos para dar tempo de aparecer o balão verde/vermelho na tela
            Thread.Sleep(18000);
        }
        
        [Given(@"Navega até a area administrativa")]
        public void DadoNavegaAteAAreaAdministrativa()
        {
            Browser.ClicarLinkPorTexto("Meus Eventos");
            Thread.Sleep(8000);
        }

        [Given(@"Clica em novo evento")]
        public void DadoClicaEmNovoEvento()
        {
            Browser.ClicarBotaoPorId("novoEvento");
        }
        
        [Given(@"preenche o formulário com os valores")]
        public void DadoPreencheOFormularioComOsValores(Table table)
        {
            string dataInicio   = table.Rows[4][1];
            string dataFim      = table.Rows[5][1];

            Browser.PreencherTextBoxPorId(table.Rows[0][0], table.Rows[0][1]);
            Browser.PreencherTextBoxPorId(table.Rows[1][0], table.Rows[1][1]);
            Browser.PreencherTextBoxPorId(table.Rows[2][0], table.Rows[2][1]);
            Thread.Sleep(500);
            Browser.PreencherCampoDropDown(table.Rows[3][0], table.Rows[3][1]);

            Browser.PreencherCampoData(table.Rows[4][0], dataInicio);
            Browser.PreencherCampoData(table.Rows[5][0], dataFim);

            Browser.PreencherTextBoxPorId(table.Rows[6][0], table.Rows[6][1]);
            Browser.PreencherTextBoxPorId(table.Rows[7][0], table.Rows[7][1]);
            Browser.PreencherTextBoxPorId(table.Rows[8][0], table.Rows[8][1]);
            Browser.PreencherTextBoxPorId(table.Rows[9][0], table.Rows[9][1]);
            Browser.PreencherTextBoxPorId(table.Rows[10][0], table.Rows[10][1]);
            Browser.PreencherTextBoxPorId(table.Rows[11][0], table.Rows[11][1]);
            Browser.PreencherTextBoxPorId(table.Rows[12][0], table.Rows[12][1]);
            Browser.PreencherTextBoxPorId(table.Rows[13][0], table.Rows[13][1]);
            Browser.PreencherTextBoxPorId(table.Rows[14][0], table.Rows[14][1]);
            Browser.PreencherTextBoxPorId(table.Rows[15][0], table.Rows[15][1]);
            Browser.PreencherCampoDropDown(table.Rows[16][0], table.Rows[16][1]);
        }

        [When(@"clicar no botao adicionar")]
        public void QuandoClicarNoBotaoAdicionar()
        {
            Browser.ClicarBotaoPorId("adicionarEvento");
            Thread.Sleep(15000);
        }

        [Then(@"O evento é registrado e o usuario redirecionado para a lista de eventos")]
        public void EntaoOEventoERegistradoEOUsuarioRedirecionadoParaAListaDeEventos()
        {
            Assert.Equal(ConfigurationHelper.Site + "eventos/meus-eventos", Browser.ObterUrl());
            Browser.ObterScreenShot("NovoEventoCadastrado");
        }
    }
}
