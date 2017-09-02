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
      public string name { get; set; }
      public int accountTypeId { get; set; }
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
          a => a.Name.Equals(newAccount.name, StringComparison.OrdinalIgnoreCase));

      if (existingAccount != null)
      {
        return BadRequest($"Account already exists with name '{newAccount.name}'.");
      }

      // Get the account type.
      AccountType accountType = _context.AccountTypes.FirstOrDefault(a => newAccount.accountTypeId == a.Id);

      if (accountType == null)
      {
        return BadRequest($"AccountType not found for id {newAccount.accountTypeId}.");
      }

      // Create new account object.
      var account = new Account
      {
        Name = newAccount.name,
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
