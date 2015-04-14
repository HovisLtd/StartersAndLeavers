using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hovis.Web.Base.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Hovis Starters and Leavers.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Hovis IS contact details.";

            return View();
        }
    }
}