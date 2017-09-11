using BoozeHoundCloud.DataTransferObjects;

namespace BoozeHoundCloud.Services
{
  public interface ITransactionService
  {
    //-------------------------------------------------------------------------

    int AddTransaction(TransactionDto newTransaction);

    //-------------------------------------------------------------------------
  }
}
