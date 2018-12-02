using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Model
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Por favor, informe o nome completo")]
        [MaxLength(100)]
        [Display(Name = "Nome Completo")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Por favor, informe o endere�o de e-mail")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [MaxLength(20)]
        [Display(Name = "Telefone")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Display(Name = "Perfis")]
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}