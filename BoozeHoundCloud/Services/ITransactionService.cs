using BoozeHoundCloud.DataTransferObjects;

namespace BoozeHoundCloud.Services
{
  public interface ITransactionService
  {
    //-------------------------------------------------------------------------

    void AddTransaction(TransactionDto newTransaction);

    //-------------------------------------------------------------------------
  }
}
