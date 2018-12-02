namespace Invoisys.Infrastructure.Data.Migrations
{
    using Context;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<InvoisysContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            CommandTimeout = int.MaxValue;
        }
        protected override void Seed(InvoisysContext context)
        {
        }
    }
}