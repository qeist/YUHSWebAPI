using System.Web.Mvc;

namespace YUHS.WebAPI.MCare.Patient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}