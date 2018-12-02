using System.Web.Mvc;

namespace Invoisys.Presentation.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index() => View();
    }
}