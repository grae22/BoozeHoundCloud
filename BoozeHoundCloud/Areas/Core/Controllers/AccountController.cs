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
    private readonly IAccountTypeService _accountTypeService;

    //-------------------------------------------------------------------------

    public AccountController()
    {
      _context = new ApplicationDbContext();
      _accountService = AccountService.Create(_context);
      _accountTypeService = AccountTypeService.Create(_context);
    }

    //-------------------------------------------------------------------------

    // Core/Account
    [HttpGet]
    [Authorize]
    public ActionResult Index()
    {
      return View("Index");
    }

    //-------------------------------------------------------------------------

    // Core/Account/AccountsOfType?typeName=X
    [HttpGet]
    [Authorize]
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
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    [ValidateAntiForgeryToken]
    public ActionResult Save(AccountDto accountDto)
    {
      if (ModelState.IsValid == false)
      {
        return New();
      }

      bool isNewAccount = (accountDto.Id == 0);

      var account = Mapper.Map<AccountDto, Account>(accountDto);

      account.AccountType = _accountTypeService.Get(accountDto.AccountTypeId);

      if (account.AccountType == null)
      {
        return HttpNotFound($"Account type not found for id {accountDto.AccountTypeId}.");
      }

      if (isNewAccount)
      {
        _accountService.AddAccount(account);
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