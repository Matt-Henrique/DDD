using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Por favor, informe o nome completo")]
        [MaxLength(100)]
        [Display(Name = "Nome Completo")]
        public string Name { get; set; }
        public bool ChangePassword { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}