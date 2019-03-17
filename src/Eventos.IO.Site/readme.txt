Instalar o Projeto a Partir do GitHub

Clonar o projeto na pasta Eventos.IO:  git clone https://github.com/CarlosValenca/Curso-de-ASP.NET-Core-e-Angular.git Eventos.IO

Abrir o projeto e aguardar restaurar as referências

No projeto 6 - Eventos.IO.TestesAutomatizados incluir 2 pacotes nugets:
1) MSTest.TestAdapter
2) MSTest.TestFramework

Eu alterei também o gitignore para que a pasta wwwroot.lib fosse enviada para o repositório de modo a podermos ver a página com o bootstrap e
demais dependências


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

31 - Docker Básico

1) Criar uma conta no Docker
2) Baixar e instalar a versão Community para Windows : https://hub.docker.com/editions/community/docker-ce-desktop-windows
3) Habilitar na BIOS a virtualização
4) Ao entrar no Windows aguardar a inicialização do Docker
5) Em settings "Shared Drives" habilitar os drives que vc deseja que fiquem a disposição dos containers criados
6) instale também o Kitematic pelo menu do Docker (vc tem que criar manualmente a pasta C:\Program Files\Docker\Kitematic e extrair os arquivos compactados para lá)
7) Para ver e controlar as Virtual Machines utilizadas pelo Docker procure no Windows por "Gerenciador do Hyper-V"
8) Vamos criar uma imagem em um primeiro container
8.1) Fazer o login: docker login (carlosvalenca - senha)
8.2) Baixar a imagem desejada, ex: docker pull hello-world
8.3) Para ver as imagens existentes basta dar o comando: "docker images"
8.4) Para colocar a imagem em um container: docker run hello-world
8.5) Se apareceu a mensagem "Hello from Docker!" é por que a instalação e configuração do Docker estão OK
8.6) O comando "docker container ls" lista os containers existentes (menos o hello-world)

32 - Incluindo uma Segunda String de Conexão Sql Server Prepando para o Docker

1) Instalar o Microsoft Sql Server Management Studio versão para Developers (que é full e troque a linguagem para inglês)

2) Instalar o Sql Server 2017 Configuration Manager

3) Crie o Login EventosIOLogin com a senha eio123

4) Configurações Importantes:
4.1) No SERVER FAMILIA2 em Security escolher a opção "Sql Server and Windows Authentication Mode"
4.2) Abra o Sql Server Configuration Manager
4.5) Procure todas as propriedades TCP/IP e habilite (TODAS)
4.4) Escolha Serviços do Sql Server e Reinicie o MSSQLSERVER
4.7) Feche o Sql Server
4.8) Ao reabrir conecte-se com o Login (EventosIOLogin) + Senha (eio123)

5) Mude a string de conexão passando o seu iplocal + "," + a porta (ipconfig o primeiro 192.168....,1433 por exemplo)
5.1) Rode todas as Migrations, para o Identity é mais fácil criar um projeto temporário com autenticação, gere as migrations e apague o projeto (cadastre a claim etc...)
5.2) Rode as demais migrations tb

6) Teste o swagger e o serviço de categorias por exemplo
6.1) Se estiver funcionando vc está pronto para o próximo capítulo

7) Como agora temos 2 strings de conexão atente para o seguinte:
7.1) Qdo vc quiser testar o LocalDB olhe nas strings de conexão e coloque no comando UseSqlServer no GetConnectionString o valor "DefaultConnection"
7.2) Qdo vc quiser testar o SqlServer olhe nas strings de conexão e coloque no comando UseSqlServer no GetConnectionString o valor "SqlServerConnection",
para o serviço e o Docker funcionar precisa ser esta a string de conexão
7.3) Ao trocar a string de conexão fazer um build da solução inteira para que os appsettings sejam jogados para a pasta debug bin
7.4) Também faça um logout e um novo login para poder ver e alterar os dados do banco correto

8) Algumas consultas e configurações adicionais no SQL Server que podem ser uteis

Para adicionar um usuário ao banco de dados do SQL Server você tem que seguir três passos:
 
Primeiro: você deve criar um login, que é um "cara" que tem permisssão de se logar no SQL Sever
 
CREATE LOGIN EventosIOLogin WITH PASSWORD = 'eio123';
 
Segundo: você deve criar um usuário para o banco de dados que deseja mapeando esse usuário para o login criado, assim seu usuário conseguirá se logar no SQL Server e entrar no banco de dados desejado.
 
CREATE USER EventosIOUser FROM LOGIN EventosIOLogin;
 
Terceiro: você deve dar ou remover permissões ao usuário porque até o segundo passo o usuário criado só tem direito a entrar no banco de dados, dando as permissões o usuário já pode operar no banco de dados. Se o usuário for comum você pode adicioná-lo apenas as roles de db_reader e db_writer, que permitirá que o usuário faça select, insert, delete e update em todas as tabelas do referido banco de dados.
 
EXEC SP_ADDROLEMEMBER 'DB_DATAREADER', 'EventosIOUser'
 
EXEC SP_ADDROLEMEMBER 'DB_DATAWRITER', 'EventosIOUser'
 
Se quiser ver melhor isso na parte gráfica, pode consultar dentro do "Object Explorer" a guia "Security", dentro dela clique em "Login", botão direito em "sa", "Properties", escolha a guia "User Mapping". Aqui você verá as roles do SQL Server pra cada usuário. Caso queira saber o que dá direito a cada role procure no SQL Server Books Online.

9) Preparação para o próximo capítulo, Configurando o Firewall no Windows:
9.1) Abra o Windows Defender Firewall
9.2) No Sql Server Configuration Manager em Configuração do SQL Native Client, escolha a propriedde TCP/IP e veja o número da porta,
esta porta DEVE estar na string de conexão logo após o SERVER separado por ","
9.3) Selecione Configurações Avançadas, Regras de Entrada, Nova Regra, informe a porta acima e de permissão geral para esta porta

10) Consultas Úteis para o próximo capítulo:

Consultas Úteis para o Docker:

SELECT @@SERVERNAME,
 CONNECTIONPROPERTY('net_transport') AS net_transport,
 CONNECTIONPROPERTY('protocol_type') AS protocol_type,
 CONNECTIONPROPERTY('auth_scheme') AS auth_scheme,
 CONNECTIONPROPERTY('local_net_address') AS local_net_address,
 CONNECTIONPROPERTY('local_tcp_port') AS local_tcp_port,
 CONNECTIONPROPERTY('client_net_address') AS client_net_address

SELECT TOP 1 local_tcp_port 
FROM sys.dm_exec_connections
WHERE local_tcp_port IS NOT NULL

SELECT @@SERVERNAME