using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Models
{
  public interface IApplicationDbContext : IDisposable
  {
    //-------------------------------------------------------------------------

    IDbSet<AccountType> AccountTypes { get; set; }
    IDbSet<Account> Accounts { get; set; }
    IDbSet<Transaction> Transactions { get; set; }

    //-------------------------------------------------------------------------

    int SaveChanges();
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    //-------------------------------------------------------------------------
  }
}