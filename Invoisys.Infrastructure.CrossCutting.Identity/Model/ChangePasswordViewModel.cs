using System.ComponentModel.DataAnnotations;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Model
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "A senha deve conter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirme a nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e sua confirmação não conferem.")]
        public string ConfirmPassword { get; set; }
    }
}