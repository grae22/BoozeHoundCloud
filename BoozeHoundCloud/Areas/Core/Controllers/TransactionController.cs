using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.Areas.Core.Services;
using BoozeHoundCloud.Areas.Core.ViewModels;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Controllers
{
  public class TransactionController : Controller
  {
    //-------------------------------------------------------------------------

    private readonly IApplicationDbContext _context;
    private readonly ITransactionService _transactionService;
    private readonly IAccountService _accountService;

    //-------------------------------------------------------------------------

    public TransactionController()
    {
      _context = new ApplicationDbContext();
      _transactionService = TransactionService.Create(_context);
      _accountService = AccountService.Create(_context);
    }

    //-------------------------------------------------------------------------

    // GET: Core/Transaction
    [HttpGet]
    public ActionResult Index()
    {
      return View();
    }

    //-------------------------------------------------------------------------

    // GET: Core/Transaction/Edit
    [HttpGet]
    public ActionResult Edit(int id)
    {
      Transaction transaction = _transactionService.GetTransaction(id);

      if (transaction == null)
      {
        return HttpNotFound();
      }

      var viewModel = new TransactionFormViewModel
      {
        Id = transaction.Id,
        Value = transaction.Value,
        DebitAccountId = transaction.DebitAccountId,
        DebitAccount = transaction.DebitAccount,
        CreditAccountId = transaction.CreditAccountId,
        CreditAccount = transaction.CreditAccount,
        Accounts = _accountService.GetAll().ToList(),
        Reference = transaction.Reference,
        Description = transaction.Description,
        Date = transaction.Date,
        CreatedTimestamp = transaction.CreatedTimestamp,
        ProcessedTimestamp = transaction.ProcessedTimestamp
      };

      return View("TransactionForm", viewModel);
    }

    //-------------------------------------------------------------------------

    // PUT: Core/Transaction/Save
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Save(Transaction transaction)
    {
      if (ModelState.IsValid == false)
      {
        return Edit(transaction.Id);
      }

      _transactionService.UpdateTransaction(transaction);

      return RedirectToAction("Index");
    }

    //-------------------------------------------------------------------------
  }
}