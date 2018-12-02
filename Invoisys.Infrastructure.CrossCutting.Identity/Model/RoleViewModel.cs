using System.ComponentModel.DataAnnotations;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Model
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nome do Perfil")]
        public string Name { get; set; }
    }
}