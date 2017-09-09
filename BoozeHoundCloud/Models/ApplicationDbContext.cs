using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Models
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,
                                      IApplicationDbContext
  {
    //-------------------------------------------------------------------------

    public IDbSet<AccountType> AccountTypes { get; set; }
    public IDbSet<Account> Accounts { get; set; }
    public IDbSet<Transaction> Transactions { get; set; }

    //-------------------------------------------------------------------------

    public ApplicationDbContext()
    :
      base("DefaultConnection", throwIfV1Schema: false)
    {
    }

    //-------------------------------------------------------------------------

    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }

    //-------------------------------------------------------------------------
  }
}