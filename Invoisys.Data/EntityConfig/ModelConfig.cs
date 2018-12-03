using Invoisys.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Invoisys.Infrastructure.Data.EntityConfig
{
    public class ModelConfig : EntityTypeConfiguration<Model>
    {
        public ModelConfig()
        {
            Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}