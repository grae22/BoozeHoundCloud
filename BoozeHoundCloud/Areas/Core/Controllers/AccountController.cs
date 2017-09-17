using System.Linq;
using System.Web.Mvc;
using BoozeHoundCloud.Areas.Core.Models;
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
    [System.Web.Mvc.HttpGet]
    public ActionResult Index(string typeName)
    {
      return View("Index");
    }

    //-------------------------------------------------------------------------

    [System.Web.Mvc.HttpGet]
    public ActionResult AccountsOfType(string typeName)
    {
      AccountType accountType = _context.AccountTypes.FirstOrDefault(x => x.Name.Equals(typeName));

      if (accountType == null)
      {
        return HttpNotFound($"AccountType '{typeName}' not found.");
      }

      return View(accountType);
    }

    //-------------------------------------------------------------------------
  }
}