using System.Configuration;
using System.Web.Mvc;

namespace Invoisys.Presentation.Web.Filters
{
    public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["RolesAuthRedirectUrl"]);
            }
        }
    }
}