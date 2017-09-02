using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Controllers.Api
{
  public class AccountController : ApiController
  {
    //-------------------------------------------------------------------------

    public struct NewAccount
    {
      public string Name { get; set; }
      public int AccountTypeId { get; set; }
    }

    //-------------------------------------------------------------------------

    private readonly ApplicationDbContext _context = new ApplicationDbContext();

    //-------------------------------------------------------------------------

    [HttpPost]
    public IHttpActionResult CreateAccount(NewAccount newAccount)
    {
      // Account already exists with name?
      Account existingAccount =
        _context.Accounts.FirstOrDefault(
          a => a.Name.Equals(newAccount.Name, StringComparison.OrdinalIgnoreCase));

      if (existingAccount != null)
      {
        return BadRequest($"Account already exists with name '{newAccount.Name}'.");
      }

      // Get the account type.
      AccountType accountType = _context.AccountTypes.FirstOrDefault(a => newAccount.AccountTypeId == a.Id);

      if (accountType == null)
      {
        return BadRequest($"AccountType not found for id {newAccount.AccountTypeId}.");
      }

      // Create new account object.
      var account = new Account
      {
        Name = newAccount.Name,
        AccountType = accountType,
        AccountTypeId = accountType.Id
      };

      _context.Accounts.Add(account);
      _context.SaveChanges();

      return StatusCode(HttpStatusCode.NoContent);
    }

    //-------------------------------------------------------------------------
  }
}
