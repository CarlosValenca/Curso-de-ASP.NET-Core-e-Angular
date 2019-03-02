using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Eventos.IO.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Eventos.IO.Domain.Organizadores.Commands;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Eventos.IO.Domain.Organizadores.Repository;
using Eventos.IO.Infra.CrossCutting.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MediatR;

using Microsoft.Extensions.Options;

namespace Eventos.IO.Services.Api.Controllers
{
    // Esta Api serve para a criação autenticaçao e token do usuário
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IOrganizadorRepository _organizadorRepository;
        private readonly IMediatorHandler _mediator;

        private readonly TokenDescriptor _tokenDescriptor;

        public AccountController(
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                ILoggerFactory loggerFactory,
                TokenDescriptor tokenDescriptor,
                IOptions<JwtTokenOptions> jwtTokenOptions,
                INotificationHandler<DomainNotification> notifications,
                // ssbcvp - parei aqui - mediator
                IUser user,
                IOrganizadorRepository organizadorRepository,
                IMediatorHandler mediator) : base(notifications, user, mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
            _organizadorRepository = organizadorRepository;
            _tokenDescriptor = tokenDescriptor;

            // ssbcvp - voltar aqui - Eduardo tirou estas validações...
            // ThrowIfInvalidOptions(_jwtTokenOptions);

            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        [HttpPost]
        [AllowAnonymous]
        [Route("nova-conta")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model, int version)
        {
            // O código abaixo é só um exemplo, vc pode definir códigos diferentes conforme a versão
            // colocando-os em métodos diferentes que serão chamados sempre do mesmo método Register
            if (version == 2)
            {
                return Response(new { Message = "API V2 não disponível " });
            }
            
            // Não deu certo e o que vc mandou foi isto aqui (model)
            if (!ModelState.IsValid) return Response(model);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Senha);

            if (result.Succeeded)
            {

                await _userManager.AddClaimAsync(user, new Claim("Eventos", "Ler"));
                await _userManager.AddClaimAsync(user, new Claim("Eventos", "Gravar"));

                var registroCommand = new RegistrarOrganizadorCommand(Guid.Parse(user.Id), model.Nome, model.CPF, user.Email);
                _mediator.EnviarComando(registroCommand);

                // Se o organizador não for criado por algum motivo apaga também o usuário criado no Identity
                if (!OperacaoValida())
                {
                    await _userManager.DeleteAsync(user);
                    return Response(model);
                }

                // O número 1 sou eu quem defino, posso definir números diferentes para mensagens diferentes
                _logger.LogInformation(1, "Usuário criado com sucesso!");
                var response = await GerarTokenUsuario(new LoginViewModel { Email = model.Email, Senha = model.Senha });
                return Response(response);
            }

            AdicionarErrosIdentity(result);
            return Response(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("conta")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotificarErroModelInvalida();
                return Response(model);
            }

            // Estou fazendo um login baseado no usuário e senha, o terceiro parametro false determina que não será
            // guardado cookie com a informação do usuário e senha e o último parâmetro determina se o usuário
            // ficará travado após X tentativas inválidas
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, true);

            if(result.Succeeded)
            {
                _logger.LogInformation(1, "Usuário logado com sucesso");

                // Só vou pegar um Token para o usuário caso o mesmo tenha sido gerado com sucesso
                // Eu já passo tb o usuário e a senha para deixar o usuário logado visto que o login foi feito com sucesso
                // var response = GerarTokenUsuario(new LoginViewModel { Email = model.Email, Password = model.Password });
                var response = await GerarTokenUsuario(model);

                return Response(response);
            }

            NotificarErro(result.ToString(), "Falha ao realizar o login");
            return Response(model);
        }

        // Vou gerar o token para o usuário
        private async Task<object> GerarTokenUsuario(LoginViewModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            var userClaims = await _userManager.GetClaimsAsync(user);

            // DICA: Se vc for a definição do JwtRegisteredClaimNames poderá ver as explicações específicas de Sub, Jti, IaT entre outras claims que vc poderá usar
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            // Necessário converver para IdentityClaims
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(userClaims);

            var handler = new JwtSecurityTokenHandler();
            var signingConf = new SigningCredentialsConfiguration();

            // Aqui de fato irei gerar o Token
            // LEMBRETE: No appsettings.json tem determinado o Emissor do Token e quais sites aceitarão o mesmo !
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenDescriptor.Issuer,
                Audience = _tokenDescriptor.Audience,
                SigningCredentials = signingConf.SigningCredentials,
                Subject = identityClaims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid)
            });

            // Criar o enconding deste token
            var encodedJwt = handler.WriteToken(securityToken);
            var orgUser = _organizadorRepository.ObterPorId(Guid.Parse(user.Id));

            // Estou devolvendo o usuário como uma informação extra que pode ser usada pelo Front End por exemplo
            var response = new
            {
                access_token = encodedJwt,
                expires_in = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid),
                            user = new
                            {
                                id = user.Id,
                                nome = orgUser.Nome,
                                email = orgUser.Email,
                                claims = userClaims.Select(c => new { c.Type, c.Value })
                            }
            };

            return response;

        }

        // Validações para o Token
        private static void ThrowIfInvalidOptions(JwtTokenOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentNullException("Must be a non-zero TimeSpan.", nameof(JwtTokenOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtTokenOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtTokenOptions.JtiGenerator));
            }
        }
    }
}