using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Infra.CrossCutting.Identity.Models;
using Eventos.IO.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Eventos.IO.Site.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        // Identity: Video 16 Eduardo Pires
        private readonly IDomainNotificationHandler<DomainNotification> _notifications;
        private readonly IOrganizadorAppService _organizadorAppService;
        private readonly IUser _user;

        // Identity: Sugestão dada por Patrick para não ter que herdar de BaseController
        protected bool OperacaoValida()
        {
            // Quando não houver notificações a operação será válida
            return (!_notifications.HasNotifications());
        }

        // Identity: Video 16 Eduardo Pires
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            // Identity: Video 16 Eduardo Pires
            IDomainNotificationHandler<DomainNotification> notifications,
            IOrganizadorAppService organizadorAppService,
            IUser user) //  : base(notifications) -- Identity: Não estamos mandando as notificações para BaseControler
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            // Identity: Video 16 Eduardo Pires - (não consegui passar notifications como : base(notifications)
            _notifications = notifications;
            _organizadorAppService = organizadorAppService;
            _user = user;
        }

        // Identity: Video 16 Eduardo Pires - Exclui a propriedade InputModel, substituindo-a por RegisterViewModel
        // Com o BindProperty vc vincula a RegisterViewModel nesta RazorPage
        // public InputModel Input { get; set; }
        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        // Identity: Video 16 Eduardo Pires - Tb substitui input por model
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };

                // Exatamente aqui o usuário será criado
                var result = await _userManager.CreateAsync(user, Input.Senha);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("Eventos", "Ler"));
                    await _userManager.AddClaimAsync(user, new Claim("Eventos", "Gravar"));
                    
                    // Identity: Video 16 Eduardo Pires
                    // Atrelei o guid do usuário do Identity ao organizador
                    // Este ponto é responsável por incluir o organizador no banco com o mesmo Id criado pelo Identity
                    var organizador = new OrganizadorViewModel
                    {
                        Id = Guid.Parse(user.Id),
                        Email = user.Email,
                        Nome = Input.Nome,
                        CPF = Input.CPF
                    };

                    // Identity: Video 16 Eduardo Pires - Este ponto é responsável por incluir o organizador no banco com o mesmo Id criado pelo Identity
                    _organizadorAppService.Registrar(organizador);

                    // Identity: Video 16 Eduardo Pires
                    if (!OperacaoValida())
                    {
                        await _userManager.DeleteAsync(user);
                        return Page();
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // Identity: Correção da rota do identity
                    var callbackUrl = Url.Page(
                        "/Identity/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    // Identity: Video 16 Eduardo Pires -Troquei input por model - 03022019
                    /* ssbcvp - voltar aqui - Envio de email ainda não está funcionando
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    */
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
