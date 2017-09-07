using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Models
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,
                                      IApplicationDbContext
  {
    //-------------------------------------------------------------------------

    public virtual IDbSet<AccountType> AccountTypes { get; set; }
    public virtual IDbSet<Account> Accounts { get; set; }
    public virtual IDbSet<Transaction> Transactions { get; set; }

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