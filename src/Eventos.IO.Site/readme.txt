email: carlos_valenca@uol.com.br
email Organizador: organizador@me.com
password: Temp@123

Para fazer um merge da versão atual com a versão do repositório remoto sem apagar ele:
git push -f origin master

install-package System.ComponentModel.Annotations no Package Manager Console lembrando a
importancia do Default project: 3 - Application\Eventos.IO.Application

install-package dapper

No método EventoFactory pode colocar o organizador manualmente
organizadorId = Guid.Parse("40c7fee0-7125-4525-a8c1-9bf9bb1b39e9")

Inclusão do Toastr no _Layout.cshtml dentro de Shared em Eventos.IO.Site
<link rel="stylesheet" href="http://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css" />
<script src="http://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
Para fazer as configurações do Toastr basta escolher no site abaixo e colar na página que vc deseja mostrar o popup:
https://codeseven.github.io/toastr/demo.html

Para forçar o Model State da View Model ficar inválido vc pode usar o comando abaixo na Immediate Window:
ModelState.AddModelError("","Erro na validação XPTO")

Um detalhe interessante, as annotations que valida nome e outras coisas não validam a parte numérica
aqui o macete é inspecionar a página para ver no próprio html qual é a origem da mensagem em inglês
que valida número não preenchido e número em formato inválido, e então alterar no próprio html
como o exemplo a seguir:
<input data-val-number="O valor está em formato inválido" data-val-required="O valor é requerido" asp-for="Valor" class="form-control" />

Para funcionar os claims é necessário instalar o pacote abaixo dentro do projeto 4-Domain\Eventos.IO.Domain:
install-package System.Security.Claims

Para aprender a incluir novos campos no identity:
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/add-user-data?view=aspnetcore-2.2&tabs=visual-studio
Para aprender a configurar as rotas no identity:
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.2&tabs=visual-studio

Instalando o Elmah em 1 - Presentation\Eventos.IO.Site
Install-Package Elmah.Io.AspNetCore
Install-Package Elmah.Io.Extensions.Logging
E necessário também injetar o LoggerFactore no método Configure para funcionar

Instalando o Elmah em 5 - Infra\5.2 - CrossCutting\Eventos.Io.Infra.CrossCutting.AspNetFilters para auditoria
install-package elmah.io.client
install-package autorest (precisei incluir manualmente no csproj pelo <PackageReference Include="autorest" Version="0.17.3" />)
install-package Microsoft.Rest.ClientRuntime

Instalar em 5 - Infra\5.2 - CrossCutting\Eventos.Io.Infra.CrossCutting.AspNetFilters
install-package microsoft.aspnetcore.http.abstractions
install-package microsoft.aspnetcore.http.extensions

Ex. de pesquisas no Elmah:
method:POST AND user:organizador02@me.com

Instalar o Automapper em 2 - Services\Eventos.IO.Services.Api
install-package automapper.Extensions.Microsoft.DependencyInjection

Instalar o pacote abaixo em 2 - Services\Eventos.IO.Services.Api:
install-package Microsoft.AspNetCore.Mvc.Formatters.Xml

Para o Swagger funcionar instalar em 2 - Services\Eventos.IO.Services.Api:
install-package Swashbuckle.AspNetCore

Para funcionar o Token nas Apis, é necessário instalar os pacotes abaixo em 5 - Infra Eventos.IO.Infra.CrossCutting.Identity
e também no meu projeto 02 - Services
install-package Microsoft.AspNetCore.Authentication.JwtBearer

06 - Testes
install-package MOQg

Para os testes integrados precisamos criar um servidor de teste
install-package Microsoft.AspNetCore.TestHost

Para os testes automatizados (que incluem regras de negócios e Front End Angular):
Vá para Tools - Extensions and Updates - Online - specflow
No projeto 6 - Eventos.IO.TestesAutomatizados:

install-package xunit
install-package xunit.runner.visualstudio (não é nativo do visual studio)

install-package specflow (define o cenário e a funcionalidade com base na história escrita conforme informado pelo usuário)
install-package specflow.xunit

Instalar o Selenium para automatizar este processo:

install-package selenium.support (funcionalidades de navegação)
install-package selenium.webdriver
install-package selenium.webdriver.chromedriver (estou instalando um browser mini aqui usado pelo selenium apenas para os testes automatizados, é possível instalar para outros drivers também)

-- Estamos usando esta package extra por conta do comando obsoleto ExpectedConditions
install-package DotNetSeleniumExtras.WaitHelpers

Este chromedriver pode ser visto na pasta: C:\Carlos\VisualStudio\Projeto\Eventos.IO\packages
Na pasta indicada a seguir poderá ser visto o mini browser citado: C:\Carlos\VisualStudio\Projeto\Eventos.IO\packages\Selenium.WebDriver.ChromeDriver.2.46.0\driver\win32
No arquivo Package.config você poderá lembrar dos pacotes instalados neste projeto

Instruções para chamar um server para testar o comportamento dos testes automatizados do Angular via browser:
1) Abrir o Cmder
2) Entrar neste diretório: C:\Carlos\VisualStudio\Projeto\Eventos.IO\src\Eventos.IO.Services.Api
3) Digitar este comando:  SET ASPNETCORE_URLS=http://*:44391 && dotnet run (o 44391 observar no angular o que ele está indo buscar)
4) Eu precisei alterar o program.cs do projeto de Serviços incluindo a seguinte linha de comando: .UseUrls("http://*:8285")
5) Para confirmar que deu certo tente obter as categorias no site: http://localhost:44391/swagger/index.html
6) Com isto estamos rodando uma sessão Self Hosting fora do Visual Studio permitindo assim rodar o teste no Test Explorer

Para conversão do test case em métodos de teste:
1) Vá para o arquivo "CadastroDeOrganizador.feature" e selecione com o botão direito do mouse a opção "Generate Steps Definition"
2) Aqui vc tem 2 opções
2.1) Gerar um arquivo novo (caso o caso de teste ainda não tenha sido criado)
2.2) Copiar para a área de transferência métodos novos criados por novos casos de testes incluídos (para então colar no caso de teste já existente)
3) Perceba que para haver a reutilização dos métodos já existentes o texto de uma ação específica precisa ser igual nestes novos métodos

Os campos Data Inicio e Data Fim tem uma forma diferente de pegar o Id:
1) Abra a aplicação em angular na página do cadastro de um novo evento
2) Inspecione a página e em especial uma das duas datas
3) Clique para copiar e copiar XPath, vc vai obter um valor similar a este: //*[@id="dataInicio"]/div/div/input (Dado o id e a data de início eu vou descer 1, 2 Divs e pegar o input que está lá dentro)
4) Este valor poderá ser utilizado no cenário de testes para determinar o campo Data Início ou Data Fim