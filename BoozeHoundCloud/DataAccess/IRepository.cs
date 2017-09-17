using System;
using System.Linq;
using System.Linq.Expressions;

namespace BoozeHoundCloud.DataAccess 
{ 
  public interface IRepository<T> : IDisposable 
  { 
    IQueryable<T> Get(); 
    T Get(int id); 
    T Get(Expression<Func<T, bool>> where); 
    void Add(T entity); 
    void Delete(int id); 
    void Update(T entity); 
    void Save(); 
  } 
}