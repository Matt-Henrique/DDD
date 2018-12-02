using Invoisys.Domain.Interface.UoW;
using Invoisys.Infrastructure.CrossCutting.Identity;
using Invoisys.Infrastructure.CrossCutting.Identity.Context;
using Invoisys.Infrastructure.CrossCutting.Identity.Model;
using Invoisys.Infrastructure.Data.Context;
using Invoisys.Infrastructure.Data.UoW;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleInjector;
using SimpleInjector.Integration.Web;

namespace Invoisys.Infrastructure.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container)
        {
            //Identity registers
            container.Register<ApplicationDbContext>(new WebRequestLifestyle());
            container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()), new WebRequestLifestyle());
            container.Register<IRoleStore<IdentityRole, string>>(() => new RoleStore<IdentityRole>(), new WebRequestLifestyle());
            container.Register<ApplicationRoleManager>(new WebRequestLifestyle());
            container.Register<ApplicationUserManager>(new WebRequestLifestyle());
            container.Register<ApplicationSignInManager>(new WebRequestLifestyle());
        }
        public static void RegisterServicesAPI(Container container)
        {
            //Identity registers
            container.Register<ApplicationDbContext>(new WebRequestLifestyle());
            container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()), new WebRequestLifestyle());
            container.Register<IRoleStore<IdentityRole, string>>(() => new RoleStore<IdentityRole>(), new WebRequestLifestyle());
            container.Register<ApplicationRoleManager>(new WebRequestLifestyle());
            container.Register<ApplicationUserManager>(new WebRequestLifestyle());

            //Invoisys Domain register
            container.Register<InvoisysContext>(new WebRequestLifestyle());
            container.Register<IUoW, UoW>(new WebRequestLifestyle());
        }
    }
}