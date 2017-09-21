using System.Web.Mvc;
using BoozeHoundCloud.Areas.Core.Services;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Controllers
{
  public class TransactionController : Controller
  {
    //-------------------------------------------------------------------------

    private readonly IApplicationDbContext _context;
    private readonly ITransactionService _transactionService;

    //-------------------------------------------------------------------------

    public TransactionController()
    {
      _context = new ApplicationDbContext();
      _transactionService = TransactionService.Create(_context);
    }

    //-------------------------------------------------------------------------

    // GET: Core/Transaction
    [HttpGet]
    public ActionResult Index()
    {
      return View();
    }

    //-------------------------------------------------------------------------
  }
}