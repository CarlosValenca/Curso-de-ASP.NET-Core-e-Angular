using Eventos.IO.TestesAutomatizados.Config;
using TechTalk.SpecFlow;
using Xunit;

// Importante: Para este arquivo ser gerado é necessário clicar com o botão direito do mouse DENTRO do arquivo CadastroDeOrganizador.feature para ver
// o menu de contexto Generate...
// Importante 2: Se vc pedir para regerar esta funcionalidade este arquivo será apagado !!!
namespace Eventos.IO.TestesAutomatizados.CadastroOrganizador
{
    [Binding]
    public class CadastroDeOrganizadorSteps
    {
        /******************************* Cenários Compartilhados *******************************/
        public SeleniunHelper Browser;

        public CadastroDeOrganizadorSteps()
        {
            Browser = SeleniunHelper.Instance();
        }

        // AAA > Arrange, Act, Assert

        // Arrange

        [Given(@"que o Organizador está no site, na página inicial")]
        public void DadoQueOOrganizadorEstaNoSiteNaPaginaInicial()
        {
            var url = Browser.NavegarParaUrl(ConfigurationHelper.SiteUrl);
            // Para que este teste seja um sucesso é necessário que a Url corrente seja a Home ! Do contrário este teste falhará
            Assert.Equal(ConfigurationHelper.SiteUrl, url);
        }

        [Given(@"clica no link de registro")]
        public void DadoClicaNoLinkDeRegistro()
        {
            Browser.ClicarLinkPorTexto("Registre-se");
        }

        [Given(@"preenche os campos com os valores")]
        public void DadoPreencheOsCamposComOsValores(Table table)
        {
            // Estou preenchendo os valores para registrar um novo Organizador
            Browser.PreencherTextBoxPorId(table.Rows[0][0], table.Rows[0][1]);
            Browser.PreencherTextBoxPorId(table.Rows[1][0], table.Rows[1][1]);
            Browser.PreencherTextBoxPorId(table.Rows[2][0], table.Rows[2][1]);
            Browser.PreencherTextBoxPorId(table.Rows[3][0], table.Rows[3][1]);
            Browser.PreencherTextBoxPorId(table.Rows[4][0], table.Rows[4][1]);
        }

        // Act

        [When(@"clicar no botão registrar")]
        public void QuandoClicarNoBotaoRegistrar()
        {
            Browser.ClicarBotaoPorId("Registrar");
        }

        /******************************* Cenários de Sucesso *******************************/
        // Assert

        [Then(@"Será registrado e redirecionado para a página home com sucesso")]
        public void EntaoSeraRegistradoERedirecionadoParaAPaginaHomeComSucesso()
        {
            var returnText = Browser.ObterTextoElementoPorId("saudacaoUsuario");
            Assert.True(returnText.ToLower().Contains("olá marcos paulo"), returnText.ToLower());
            Browser.ObterScreenShot("EvidenciaCadastro");
        }

        /******************************* Cenários de Falha *******************************/
        [Then(@"Recebe uma mensagem de erro de CPF já cadastrado")]
        public void EntaoRecebeUmaMensagemDeErroDeCPFJaCadastrado()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("cpf ou e-mail já utilizados", result.ToLower());

            Browser.ObterScreenShot("CPF_Erro");
        }

        [Then(@"recebe uma mensagem de erro de email já cadastrado")]
        public void EntaoRecebeUmaMensagemDeErroDeEmailJaCadastrado()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("is already taken", result.ToLower());

            Browser.ObterScreenShot("Email_Erro");
        }

        [Then(@"Recebe uma mensagem de erro de que a senha requer numero")]
        public void EntaoRecebeUmaMensagemDeErroDeQueASenhaRequerNumero()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("passwords must have at least one digit ('0'-'9')", result.ToLower());
            Browser.ObterScreenShot("Senha_Numero_Erro");
        }

        [Then(@"Recebe uma mensagem de erro de que a senha requer letra maiuscula")]
        public void EntaoRecebeUmaMensagemDeErroDeQueASenhaRequerLetraMaiuscula()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("passwords must have at least one uppercase ('a'-'z')", result.ToLower());
            Browser.ObterScreenShot("Senha_Maiuscula_Erro");
        }

        [Then(@"Recebe uma mensagem de erro de que a senha requer letra minuscula")]
        public void EntaoRecebeUmaMensagemDeErroDeQueASenhaRequerLetraMinuscula()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("passwords must have at least one lowercase ('a'-'z')", result.ToLower());
            Browser.ObterScreenShot("Senha_Minuscula_Erro");
        }

        [Then(@"Recebe uma mensagem de erro de que a senha requer caracter especial")]
        public void EntaoRecebeUmaMensagemDeErroDeQueASenhaRequerCaracterEspecial()
        {
            var result = Browser.ObterTextoElementoPorClasse("alert-danger");
            Assert.Contains("passwords must have at least one non alphanumeric character", result.ToLower());
            Browser.ObterScreenShot("Senha_Caracter_Especial_Erro");
        }

        [Then(@"Recebe uma mensagem de erro de que a senha esta em tamanho inferior ao permitido")]
        public void EntaoRecebeUmaMensagemDeErroDeQueASenhaEstaEmTamanhoInferiorAoPermitido()
        {
            var result = Browser.ObterTextoElementoPorClasse("text-danger");
            Assert.Contains("a senha precisa ter pelo menos 6 caracteres", result.ToLower());
            Browser.ObterScreenShot("Senha_Pequena_Erro");
        }

        [Then(@"Recebe uma mensagem de erro de que a senha estao diferentes")]
        public void EntaoRecebeUmaMensagemDeErroDeQueASenhaEstaoDiferentes()
        {
            // Força aparecer a mensagem de erro Senhas Diferentes...
            Browser.SelecionaCampoTexto("senha");
            var result = Browser.ObterTextoElementoPorClasse("text-danger");
            Assert.Contains("as senhas não conferem", result.ToLower());
            Browser.ObterScreenShot("Senha_Diferente_Erro");
        }
    }
}
