using Invoisys.Infrastructure.CrossCutting.Identity.Model;
using System.Data.Entity.ModelConfiguration;

namespace Invoisys.Infrastructure.CrossCutting.Identity.EntityConfig
{
    public class ApplicationUserConfig : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfig()
        {
            Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}