using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.DAL
{
  public class AccountTypeRepository : IRepository<AccountType>
  {
    //-------------------------------------------------------------------------

    private readonly ApplicationDbContext _context;
    private bool _disposed;

    //-------------------------------------------------------------------------

    public AccountTypeRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    //-------------------------------------------------------------------------

    public IEnumerable<AccountType> Get()
    {
      return _context.AccountTypes.ToList();
    }

    //-------------------------------------------------------------------------

    public AccountType Get(int id)
    {
      return _context.AccountTypes.Find(id);
    }

    //-------------------------------------------------------------------------

    public AccountType Get(Expression<Func<AccountType, bool>> where)
    {
      return _context.AccountTypes.FirstOrDefault(where);
    }

    //-------------------------------------------------------------------------

    public void Add(AccountType accountType)
    {
      _context.AccountTypes.Add(accountType);
    }

    //-------------------------------------------------------------------------

    public void Delete(int id)
    {
      _context.AccountTypes.Remove(Get(id));
    }

    //-------------------------------------------------------------------------

    public void Update(AccountType accountType)
    {
      _context.Entry(accountType).State = EntityState.Modified;
    }

    //-------------------------------------------------------------------------

    public void Save()
    {
      _context.SaveChanges();
    }

    //-------------------------------------------------------------------------

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    //-------------------------------------------------------------------------

    protected virtual void Dispose(bool disposing)
    {
      if (_disposed)
      {
        return;
      }

      if (disposing)
      {
        _context.Dispose();
      }

      _disposed = true;
    }

    //-------------------------------------------------------------------------
  }
}