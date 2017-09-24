using System.Linq;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public interface ITransactionService
  {
    //-------------------------------------------------------------------------

    int AddTransaction(Transaction newTransaction);
    void UpdateTransaction(Transaction modifiedTransaction);
    IQueryable<Transaction> GetAll();
    Transaction GetTransaction(int id);

    //-------------------------------------------------------------------------
  }
}
