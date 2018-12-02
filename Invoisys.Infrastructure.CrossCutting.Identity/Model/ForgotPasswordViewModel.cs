using System.ComponentModel.DataAnnotations;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Model
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
