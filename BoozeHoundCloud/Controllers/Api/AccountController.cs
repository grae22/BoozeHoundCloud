using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using BoozeHoundCloud.Dtos;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Controllers.Api
{
  public class AccountController : ApiController
  {
    //-------------------------------------------------------------------------

    private readonly IApplicationDbContext _context;

    //-------------------------------------------------------------------------

    public AccountController()
    {
      _context = new ApplicationDbContext();
    }

    //-------------------------------------------------------------------------

    public AccountController(IApplicationDbContext context)
    {
      _context = context;
    }

    //-------------------------------------------------------------------------
    
    [HttpGet]
    public IHttpActionResult GetAccount(int id)
    {
      // Account already exists with name?
      IAccount account = _context.Accounts.Find(id);

      if (account == null)
      {
        return NotFound();
      }

      var accountDto = Mapper.Map<IAccount, AccountDto>(account);

      return Json(accountDto);
    }

    //-------------------------------------------------------------------------

    [HttpPost]
    public IHttpActionResult CreateAccount(AccountDto accountDto)
    {
      // Account already exists with name?
      IAccount existingAccount =
        _context.Accounts.FirstOrDefault(
          a => a.Name.Equals(accountDto.Name, StringComparison.OrdinalIgnoreCase));

      if (existingAccount != null)
      {
        return BadRequest($"Account already exists with name '{accountDto.Name}'.");
      }

      // Get the account type.
      IAccountType accountType = _context.AccountTypes.Find(accountDto.AccountTypeId);

      if (accountType == null)
      {
        return BadRequest($"AccountType not found for id {accountDto.AccountTypeId}.");
      }

      // Create new account object.
      var newAccount = Mapper.Map<AccountDto, Account>(accountDto);

      _context.Accounts.Add(newAccount);
      _context.SaveChanges();

      return Created(
        new Uri(
          $"{Request.RequestUri}/{newAccount.Id}"),
          newAccount);
    }

    //-------------------------------------------------------------------------
  }
}
