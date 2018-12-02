using System.ComponentModel.DataAnnotations;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Model
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "A senha deve conter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "A senha e sua confirmação não conferem.")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}