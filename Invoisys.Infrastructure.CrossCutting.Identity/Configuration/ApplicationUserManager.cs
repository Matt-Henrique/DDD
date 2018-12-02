using Invoisys.Infrastructure.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Configuration
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonLetterOrDigit = false,
            };

            UserLockoutEnabledByDefault = true;
            MaxFailedAccessAttemptsBeforeLockout = 5;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);

            EmailService = new EmailService();
            var provider = new DpapiDataProtectionProvider("Invoisys");
            var dataProtector = provider.Create("ASP.NET Identity");
            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtector);
        }
        public string PasswordDefault() => "123Mudar";
    }
}