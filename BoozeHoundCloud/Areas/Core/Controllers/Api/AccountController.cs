using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.Areas.Core.Services;

namespace BoozeHoundCloud.Areas.Core.Controllers.Api
{
  public class AccountController : ApiController
  {
    //-------------------------------------------------------------------------

    private readonly IAccountService _accountService;
    private readonly IAccountTypeService _accountTypeService;

    //-------------------------------------------------------------------------

    public AccountController()
    {
      var context = new ApplicationDbContext();
      var accounts = new GenericRepository<Account>(context);
      var accountTypes = new GenericRepository<AccountType>(context);
      var interAccountMappings = new GenericRepository<InterAccountTypeTransactionMapping>(context);
      _accountTypeService = new AccountTypeService(interAccountMappings);

      _accountService = new AccountService(accounts, accountTypes, _accountTypeService);
    }

    //-------------------------------------------------------------------------

    // Constructor provided for unit-testing, could be removed if dependency injection is used.

    internal AccountController(IAccountService accountService,
                               IAccountTypeService accountTypeService)
    {
      _accountService = accountService;
      _accountTypeService = accountTypeService;
    }

    //-------------------------------------------------------------------------

    [HttpGet]
    public IHttpActionResult GetAll(int? typeId = null)
    {
      if (typeId == null)
      {
        return Json(_accountService.GetAll());
      }

      return Json(
        _accountService
          .GetAll()
          .Where(acc => acc.AccountType.Id == typeId));
    }

    //-------------------------------------------------------------------------

    [HttpGet]
    public IHttpActionResult GetAccount(int id)
    {
      Account account = _accountService.GetAccount(id);

      if (account == null)
      {
        return NotFound();
      }

      var accountDto = Mapper.Map<Account, AccountDto>(account);

      return Ok(accountDto);
    }

    //-------------------------------------------------------------------------

    [HttpGet]
    [Route("Api/Core/Account/CanCredit/{id:int}")]
    public IHttpActionResult GetAccountsAccountCanCredit(int id)
    {
      Account fromAccount = _accountService.GetAccount(id);

      if (fromAccount == null)
      {
        return NotFound();
      }

      List<Account> accounts =
        _accountService.GetAll().Where(a =>
            _accountTypeService.IsTransferAllowed(fromAccount.AccountType, a.AccountType))
          .ToList();

      return Json(accounts);
    }

    //-------------------------------------------------------------------------

    [HttpGet]
    [Route("Api/Core/Account/CanDebit/{id:int}")]
    public IHttpActionResult GetAccountsAccountCanDebit(int id)
    {
      Account toAccount = _accountService.GetAccount(id);

      if (toAccount == null)
      {
        return NotFound();
      }

      List<Account> accounts =
        _accountService.GetAll().Where(a =>
            _accountTypeService.IsTransferAllowed(a.AccountType, toAccount.AccountType))
          .ToList();

      return Json(accounts);
    }

    //-------------------------------------------------------------------------

    [HttpPost]
    public IHttpActionResult CreateAccount(AccountDto accountDto)
    {
      try
      {
        var account = Mapper.Map<AccountDto, Account>(accountDto);

        _accountService.AddAccount(account);

        return Created(
          new Uri(
            $"{Request.RequestUri}/{account.Id}"),
            account);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    //-------------------------------------------------------------------------
  }
}
