using System.Web.Mvc;

namespace BoozeHoundCloud.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return RedirectPermanent("Core/");
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}