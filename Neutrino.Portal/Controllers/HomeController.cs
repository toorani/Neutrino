using System.Web.Mvc;
using Neutrino.Portal.Attributes;

namespace Neutrino.Portal.Controllers
{
    [ControllerAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}