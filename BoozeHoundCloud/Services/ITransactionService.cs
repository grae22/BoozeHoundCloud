using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Services
{
  public interface ITransactionService
  {
    //-------------------------------------------------------------------------

    int AddTransaction(TransactionDto newTransaction);
    Transaction GetTransaction(int id);

    //-------------------------------------------------------------------------
  }
}
