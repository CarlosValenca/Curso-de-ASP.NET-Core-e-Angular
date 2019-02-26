using System.ComponentModel.DataAnnotations;

namespace Eventos.IO.Infra.CrossCutting.Identity.Models.AccountViewModels
{
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
    public string Senha { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirme a senha")]
    [Compare("Senha", ErrorMessage = "The password and confirmation password do not match.")]
    public string SenhaConfirmacao { get; set; }
}
}