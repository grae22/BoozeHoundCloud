using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.DAL
{
  public class AccountRepository : IRepository<Account>
  {
    //-------------------------------------------------------------------------

    private readonly ApplicationDbContext _context;
    private bool _disposed;

    //-------------------------------------------------------------------------

    public AccountRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    //-------------------------------------------------------------------------

    public IEnumerable<Account> Get()
    {
      return _context.Accounts.ToList();
    }

    //-------------------------------------------------------------------------

    public Account Get(int id)
    {
      return _context.Accounts.Find(id);
    }

    //-------------------------------------------------------------------------

    public Account Get(Expression<Func<Account, bool>> where)
    {
      return _context.Accounts.FirstOrDefault(where);
    }

    //-------------------------------------------------------------------------

    public void Add(Account accountType)
    {
      _context.Accounts.Add(accountType);
    }

    //-------------------------------------------------------------------------

    public void Delete(int id)
    {
      _context.Accounts.Remove(Get(id));
    }

    //-------------------------------------------------------------------------

    public void Update(Account accountType)
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