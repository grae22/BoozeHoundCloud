using System;
using System.Web.Http;
using AutoMapper;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;
using BoozeHoundCloud.Services;

namespace BoozeHoundCloud.Controllers.Api
{
  public class AccountController : ApiController
  {
    //-------------------------------------------------------------------------

    private readonly IAccountService _accounts;

    //-------------------------------------------------------------------------

    public AccountController()
    {
      var context = new ApplicationDbContext();
      var accounts = new GenericRepository<Account>(context);
      var accountTypes = new GenericRepository<AccountType>(context);

      _accounts = new AccountService(accounts, accountTypes);
    }

    //-------------------------------------------------------------------------

    public AccountController(IAccountService accounts)
    {
      _accounts = accounts;
    }

    //-------------------------------------------------------------------------
    
    [HttpGet]
    public IHttpActionResult GetAccount(int id)
    {
      Account account = _accounts.GetAccount(id);

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
        Account newAccount = _accounts.AddAccount(accountDto);

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
