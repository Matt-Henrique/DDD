using Invoisys.Domain.Entity;
using Invoisys.Infrastructure.Data.EntityConfig;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Invoisys.Infrastructure.Data.Context
{
    public class InvoisysContext : DbContext
    {
        public InvoisysContext() : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 360;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Model> Models { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfig());
            modelBuilder.Configurations.Add(new ModelConfig());
            modelBuilder.Ignore<User>();
            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType?.Name + "Id")
                .Configure(p => p.IsKey());
            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType?.Name + "CreateDate")
                .Configure(p => p.IsRequired());
            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        /// <inheritdoc />
        /// <summary>
        /// Override SaveChanges method in InvoisysContext and get a list of Added and Modified of your entity and if it is added set CreatedDate and if it is modified set ModifiedDate.
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state when SaveChanges was called</returns>
        public override int SaveChanges()
        {
            var addedEntities = ChangeTracker.Entries<EntityBase>().Where(e => e.State == EntityState.Added).ToList();
            addedEntities.ForEach(e =>
            {
                e.Entity.SetCreated();
            });
            var modifiedEntities = ChangeTracker.Entries<EntityBase>().Where(e => e.State == EntityState.Modified).ToList();
            modifiedEntities.ForEach(e =>
            {
                // Ignore changes to the value of CreateDate
                e.Property("CreateDate").IsModified = false;
                e.Entity.SetModified();
            });
            return base.SaveChanges();
        }
    }
}