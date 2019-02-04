using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Eventos.IO.Site.Areas.Identity.Data;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
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
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
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

        // Identity: Video 16 Eduardo Pires- troquei InputModel por RegisterViewModel
        public class RegisterViewModel
        {
            // Identity: Coloque aqui os novos campos e suas validações
            [Required(ErrorMessage = "O nome é requerido")]
            [DataType(DataType.Text)]
            [Display(Name = "Nome Completo")]
            public string Nome { get; set; }

            [Required(ErrorMessage = "O CPF é requerido")]
            [StringLength(11)]
            public string CPF { get; set; }

            [Required(ErrorMessage = "O e-email é requerido")]
            [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme a senha")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

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

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {

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
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    // Identity: Video 16 Eduardo Pires -Troquei input por model - 03022019
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

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
