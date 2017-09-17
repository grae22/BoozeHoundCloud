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

    //-------------------------------------------------------------------------

    public AccountController()
    {
      var context = new ApplicationDbContext();
      var accounts = new GenericRepository<Account>(context);
      var accountTypes = new GenericRepository<AccountType>(context);
      var interAccountMappings = new GenericRepository<InterAccountTypeTransactionMapping>(context);
      var accountTypeService = new AccountTypeService(interAccountMappings);

      _accountService = new AccountService(accounts, accountTypes, accountTypeService);
    }

    //-------------------------------------------------------------------------

    // Constructor provided for unit-testing, could be removed if dependency injection is used.

    internal AccountController(IAccountService accountService)
    {
      _accountService = accountService;
    }

    //-------------------------------------------------------------------------

    [HttpGet]
    public IHttpActionResult GetAll()
    {
      return Ok(_accountService.GetAll());
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

    [HttpPost]
    public IHttpActionResult CreateAccount(AccountDto accountDto)
    {
      try
      {
        Account newAccount = _accountService.AddAccount(accountDto);

        if (newAccount == null)
        {
          return BadRequest($"Failed to add new account '{accountDto.Name}'.");
        }

        return Created(
          new Uri(
            $"{Request.RequestUri}/{newAccount.Id}"),
            newAccount);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    //-------------------------------------------------------------------------
  }
}
