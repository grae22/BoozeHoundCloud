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

    private readonly ApplicationDbContext _context = new ApplicationDbContext();

    //-------------------------------------------------------------------------

    [HttpGet]
    public IHttpActionResult GetAccount(int id)
    {
      // Account already exists with name?
      Account account = _context.Accounts.FirstOrDefault(a => a.Id == id);

      if (account == null)
      {
        return NotFound();
      }

      var accountDto = Mapper.Map<Account, AccountDto>(account);

      return Json(accountDto);
    }

    //-------------------------------------------------------------------------

    [HttpPost]
    public IHttpActionResult CreateAccount(AccountDto accountDto)
    {
      // Account already exists with name?
      Account existingAccount =
        _context.Accounts.FirstOrDefault(
          a => a.Name.Equals(accountDto.Name, StringComparison.OrdinalIgnoreCase));

      if (existingAccount != null)
      {
        return BadRequest($"Account already exists with name '{accountDto.Name}'.");
      }

      // Get the account type.
      AccountType accountType = _context.AccountTypes.FirstOrDefault(a => accountDto.AccountTypeId == a.Id);

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
