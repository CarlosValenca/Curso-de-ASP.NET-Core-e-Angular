Funcionalidade: Cadastro de Organizador
	Um organizador fará seu cadastro pelo site
	para poder gerenciar seus próprios eventos
	Ao terminar o cadastro receberá uma notificação
	de sucesso ou de falha.

@TesteAutomatizadoCadastroDeOrganizadorComSucesso

Cenário: Cadastro de Organizador com Sucesso
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo				| Valor					|
		| nome				| Marcos Paulo			|
		| cpf				| 67926852082			|
		| email				| marcospaulo@me.com	|
		| senha				| Temp@123				|
		| senhaConfirmacao	| Temp@123				|
	Quando clicar no botão registrar
	Então Será registrado e redirecionado para a página home com sucesso

@TesteAutomatizadoCadastroOrganizadorFalha

Cenário: Cadastro de Organizador com CPF já utilizado
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor					|
		| nome             | Marcos Paulo Silva		|
		| cpf              | 67926852082			|
		| email            | marcospaulo2@me.com	|
		| senha            | Temp@123				|
		| senhaConfirmacao | Temp@123				|
	Quando clicar no botão registrar
	Entao Recebe uma mensagem de erro de CPF já cadastrado

@TesteAutomatizadoCadastroOrganizadorFalha

Cenário: Cadastro de Organizador com Email já utilizado
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor					|
		| nome             | Marcos Paulo Silva		|
		| cpf              | 30390600829			|
		| email            | marcospaulo@me.com		|
		| senha            | Teste@123				|
		| senhaConfirmacao | Teste@123				|
	Quando clicar no botão registrar
	Entao recebe uma mensagem de erro de email já cadastrado

@TesteAutomatizadoCadastroOrganizadorFalha

Cenário: Cadastro de Organizador com Senha sem números
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor					|
		| nome             | Marcos Paulo Silva		|
		| cpf              | 30390600829			|
		| email            | marcospaulo2@me.com	|
		| senha            | Teste@                 |
		| senhaConfirmacao | Teste@                 |
	Quando clicar no botão registrar
	Entao Recebe uma mensagem de erro de que a senha requer numero

@TesteAutomatizadoCadastroOrganizadorFalha

Cenário: Cadastro de Organizador com Senha sem Maiuscula
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor					|
		| nome             | Marcos Paula Silva		|
		| cpf              | 30390600829			|
		| email            | marcospaula2@me.com	|
		| senha            | teste@123              |
		| senhaConfirmacao | teste@123              |
	Quando clicar no botão registrar
	Entao Recebe uma mensagem de erro de que a senha requer letra maiuscula

@TesteAutomatizadoCadastroOrganizadorFalha

Cenário: Cadastro de Organizador com Senha sem minuscula
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor					|
		| nome             | Marcos Paula Silva		|
		| cpf              | 30390600829			|
		| email            | marcospaulo2@me.com	|
		| senha            | TESTE@123              |
		| senhaConfirmacao | TESTE@123              |
	Quando clicar no botão registrar		
	Entao Recebe uma mensagem de erro de que a senha requer letra minuscula

@TesteAutomatizadoCadastroOrganizadorFalha

Cenário: Cadastro de Organizador com Senha sem caracter especial
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor					|
		| nome             | Marcos Paulo Silva		|
		| cpf              | 30390600822			|
		| email            | marcospaulo2@me.com	|
		| senha            | Teste123				|
		| senhaConfirmacao | Teste123				|
	Quando clicar no botão registrar		
	Entao Recebe uma mensagem de erro de que a senha requer caracter especial

@TesteAutomatizadoCadastroOrganizadorFalha
		
Cenário: Cadastro de Organizador com Senha em tamanho inferior ao permitido
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor					|
		| nome             | Marcos Paula Silva		|
		| cpf              | 30390600822			|
		| email            | marcospaula2@me.com	|
		| senha            | Te123					|
		| senhaConfirmacao | Te123					|
	Quando clicar no botão registrar
	Entao Recebe uma mensagem de erro de que a senha esta em tamanho inferior ao permitido

@TesteAutomatizadoCadastroOrganizadorFalha

Cenário: Cadastro de Organizador com Senha diferentes
	Dado que o Organizador está no site, na página inicial
	E clica no link de registro
	E preenche os campos com os valores
		| Campo            | Valor					|
		| nome             | Marcos Paulo Silva		|
		| cpf              | 30390600822			|
		| email            | marcospaulo2@me.com	|
		| senha            | Teste@123              |
		| senhaConfirmacao | Teste@124              |
	Quando clicar no botão registrar
	Entao Recebe uma mensagem de erro de que a senha estao diferentes