using System; 
using System.Collections.Generic; 
using System.Linq.Expressions; 
 
namespace BoozeHoundCloud.DAL 
{ 
  public interface IRepository<T> : IDisposable 
  { 
    IEnumerable<T> Get(); 
    T Get(int id); 
    T Get(Expression<Func<T, bool>> where); 
    void Add(T entity); 
    void Delete(int id); 
    void Update(T entity); 
    void Save(); 
  } 
}