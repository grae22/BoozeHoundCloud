using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.Areas.Core.Services;
using BoozeHoundCloud.Areas.Core.ViewModels;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Controllers
{
  public class AccountController : Controller
  {
    //-------------------------------------------------------------------------

    private readonly IApplicationDbContext _context;
    private readonly IAccountService _accountService;

    //-------------------------------------------------------------------------

    public AccountController()
    {
      _context = new ApplicationDbContext();
      _accountService = AccountService.Create(_context);
    }

    //-------------------------------------------------------------------------

    // GET: Core/Account
    [HttpGet]
    public ActionResult Index(string typeName)
    {
      return View("Index");
    }

    //-------------------------------------------------------------------------

    // GET: Core/Account/AccountsOfType?typeName=X
    [HttpGet]
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

    // GET: Core/Account/Create
    [HttpGet]
    public ActionResult Create()
    {
      var viewModel = new AccountFormViewModel
      {
        AccountTypes = _context.AccountTypes
      };

      return View("AccountForm", viewModel);
    }

    //-------------------------------------------------------------------------

    // GET: Core/Account/Save
    [ValidateAntiForgeryToken]
    public ActionResult Save(Account account)
    {
      if (ModelState.IsValid == false)
      {
        return Create();
      }

      bool isNewAccount = (account.Id == 0);

      account.AccountType = _context.AccountTypes.FirstOrDefault(x => x.Id == account.AccountTypeId);

      if (account.AccountType == null)
      {
        return Create();
      }

      if (isNewAccount)
      {
        var accountDto = Mapper.Map<Account, AccountDto>(account);
        _accountService.AddAccount(accountDto);
      }
      else
      {
        // TODO: Update existing account.
      }

      var args = new RouteValueDictionary();
      args.Add("typeName", account.AccountType.Name);
      return RedirectToAction("AccountsOfType", "Account", args);
    }

    //-------------------------------------------------------------------------
  }
}