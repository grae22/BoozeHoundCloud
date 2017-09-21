using System.Linq;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public interface ITransactionService
  {
    //-------------------------------------------------------------------------

    int AddTransaction(TransactionDto newTransaction);
    IQueryable<Transaction> GetAll();
    Transaction GetTransaction(int id);

    //-------------------------------------------------------------------------
  }
}
