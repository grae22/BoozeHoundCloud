using System.Web.Mvc;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Controllers
{
  public class AccountController : Controller
  {
    //-------------------------------------------------------------------------

    private readonly IApplicationDbContext _context;

    //-------------------------------------------------------------------------

    public AccountController()
    {
      _context = new ApplicationDbContext();
    }

    //-------------------------------------------------------------------------

    // GET: Core/Account
    [HttpGet]
    public ActionResult Index()
    {
      return View();
    }

    //-------------------------------------------------------------------------
  }
}