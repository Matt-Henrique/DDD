using Invoisys.Infrastructure.CrossCutting.Identity.EntityConfig;
using Invoisys.Infrastructure.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ApplicationUserConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}