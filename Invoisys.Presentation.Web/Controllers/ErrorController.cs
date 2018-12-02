using System.Web.Mvc;

namespace Invoisys.Presentation.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Authorize]
        public ActionResult AccessDenied() => View();
        [AllowAnonymous]
        public ActionResult Page404() => View();
        [AllowAnonymous]
        public ActionResult Page500() => View();
    }
}