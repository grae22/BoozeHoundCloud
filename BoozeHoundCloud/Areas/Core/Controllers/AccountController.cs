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

    // Core/Account
    [HttpGet]
    public ActionResult Index()
    {
      return View("Index");
    }

    //-------------------------------------------------------------------------

    // Core/Account/AccountsOfType?typeName=X
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

    // Core/Account/New
    [HttpGet]
    public ActionResult New()
    {
      var viewModel = new AccountFormViewModel
      {
        AccountTypes = _context.AccountTypes
      };

      return View("AccountForm", viewModel);
    }

    //-------------------------------------------------------------------------

    // Core/Account/Edit
    [HttpGet]
    public ActionResult Edit(int id)
    {
      Account account = _accountService.GetAccount(id);

      if (account == null)
      {
        return HttpNotFound($"Account not found with id {id}.");
      }

      var viewModel = new AccountFormViewModel
      {
        Id = account.Id,
        Name = account.Name,
        AccountTypeId = account.AccountTypeId,
        AccountTypes = _context.AccountTypes,
        Balance = account.Balance
      };

      return View("AccountForm", viewModel);
    }

    //-------------------------------------------------------------------------

    // Core/Account/Save
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Save(Account account)
    {
      if (ModelState.IsValid == false)
      {
        return New();
      }

      bool isNewAccount = (account.Id == 0);

      if (isNewAccount)
      {
        var accountDto = Mapper.Map<Account, AccountDto>(account);
        _accountService.AddAccount(accountDto);
      }
      else
      {
        _accountService.UpdateAccount(account);
      }

      var args = new RouteValueDictionary();
      args.Add("typeName", account.AccountType.Name);
      return RedirectToAction("AccountsOfType", "Account", args);
    }

    //-------------------------------------------------------------------------
  }
}