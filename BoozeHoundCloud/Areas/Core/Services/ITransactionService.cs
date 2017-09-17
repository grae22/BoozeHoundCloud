using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public interface ITransactionService
  {
    //-------------------------------------------------------------------------

    int AddTransaction(TransactionDto newTransaction);
    Transaction GetTransaction(int id);

    //-------------------------------------------------------------------------
  }
}
