[assembly: WebActivator.PostApplicationStartMethod(typeof(Invoisys.Presentation.Web.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace Invoisys.Presentation.Web.App_Start
{
    using Invoisys.Infrastructure.CrossCutting.IoC;
    using Microsoft.Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;

    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            InitializeContainer(container);
            container.Register(() =>
            {
                if (HttpContext.Current != null && HttpContext.Current.Items["owin.Environment"] == null && container.IsVerifying)
                {
                    return new OwinContext().Authentication;
                }
                return HttpContext.Current.GetOwinContext().Authentication;
            }, new WebRequestLifestyle());
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
        private static void InitializeContainer(Container container)
        {
            BootStrapper.RegisterServices(container);
        }
    }
}