using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BoozeHoundCloud.Models;
 
namespace BoozeHoundCloud.DataAccess
{
  public class GenericRepository<T> : IRepository<T> where T : class
  {
    //-------------------------------------------------------------------------

    private readonly IApplicationDbContext _context;
    private readonly IDbSet<T> _entity;
    private bool _disposed;

    //-------------------------------------------------------------------------

    public GenericRepository(IApplicationDbContext context)
    {
      _context = context;
      _entity = _context.Set<T>();
    }

    //-------------------------------------------------------------------------

    // Constructor for unit tests.

    internal GenericRepository(IApplicationDbContext context,
                               IDbSet<T> dbSet)
    {
      _context = context;
      _entity = dbSet;
    }

    //-------------------------------------------------------------------------

    public IQueryable<T> Get()
    {
      return _entity.ToList().AsQueryable();
    }

    //-------------------------------------------------------------------------

    public T Get(int id)
    {
      return _entity.Find(id);
    }

    //-------------------------------------------------------------------------

    public T Get(Expression<Func<T, bool>> where)
    {
      return _entity.FirstOrDefault(where);
    }

    //-------------------------------------------------------------------------

    public void Add(T entity)
    {
      _entity.Add(entity);
    }

    //-------------------------------------------------------------------------

    public void Delete(int id)
    {
      _entity.Remove(Get(id));
    }

    //-------------------------------------------------------------------------

    public void Update(T entity)
    {
      _context.Entry(entity).State = EntityState.Modified;
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