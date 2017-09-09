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
 
    private readonly IApplicationDbContext _context; 
    private bool _disposed; 
 
    //------------------------------------------------------------------------- 
 
    public AccountRepository(IApplicationDbContext context) 
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
 
    public void Add(Account account) 
    { 
      _context.Accounts.Add(account); 
    } 
 
    //------------------------------------------------------------------------- 
 
    public void Delete(int id) 
    { 
      _context.Accounts.Remove(Get(id)); 
    } 
 
    //------------------------------------------------------------------------- 
 
    public void Update(Account account) 
    { 
      _context.Entry(account).State = EntityState.Modified; 
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