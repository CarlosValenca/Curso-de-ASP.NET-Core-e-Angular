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