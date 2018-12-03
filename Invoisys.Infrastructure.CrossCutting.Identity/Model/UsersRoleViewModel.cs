using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Model
{
    public class UsersRoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        [Display(Name = "Nome Completo")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Telefone")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Perfis")]
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}