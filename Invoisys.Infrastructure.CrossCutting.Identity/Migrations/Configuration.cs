namespace Invoisys.Infrastructure.CrossCutting.Identity.Migrations
{
    using Invoisys.Infrastructure.CrossCutting.Identity.Context;
    using Invoisys.Infrastructure.CrossCutting.Identity.Model;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!context.Roles.Any(r => r.Name.Equals("Admin")))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);
            }
            if (!context.Users.Any(u => u.Email.Equals("mateus.tofanello@outlook.com")))
            {
                var passwordHash = new PasswordHasher();
                var user = new ApplicationUser
                {
                    EmailConfirmed = true,
                    ChangePassword = false,
                    Name = "Mateus Henrique Tofanello",
                    Email = "mateus.tofanello@outlook.com",
                    UserName = "mateus.tofanello@outlook.com",
                    PasswordHash = passwordHash.HashPassword("123Mudar")
                };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
