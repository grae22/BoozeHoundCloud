using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace BoozeHoundCloud.DAL
{
  public class GenericRepository<T> : IRepository<T> where T : class
  {
    //-------------------------------------------------------------------------

    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;
    private bool _disposed;

    //-------------------------------------------------------------------------

    public GenericRepository(DbContext context)
    {
      _context = context;
      _dbSet = _context.Set<T>();
    }

    //-------------------------------------------------------------------------

    public IEnumerable<T> Get()
    {
      return _dbSet.ToList();
    }

    //-------------------------------------------------------------------------

    public T Get(int id)
    {
      return _dbSet.Find(id);
    }

    //-------------------------------------------------------------------------

    public T Get(Expression<Func<T, bool>> where)
    {
      return _dbSet.FirstOrDefault(where);
    }

    //-------------------------------------------------------------------------

    public void Add(T accountType)
    {
      _dbSet.Add(accountType);
    }

    //-------------------------------------------------------------------------

    public void Delete(int id)
    {
      _dbSet.Remove(Get(id));
    }

    //-------------------------------------------------------------------------

    public void Update(T accountType)
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