using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Models
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    //-------------------------------------------------------------------------

    public DbSet<AccountType> AccountTypes { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

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