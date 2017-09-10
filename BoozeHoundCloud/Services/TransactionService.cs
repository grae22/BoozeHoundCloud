using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Services
{
  internal class TransactionService : ITransactionService
  {
    //-------------------------------------------------------------------------

    private readonly IRepository<Transaction> _transactions;
    private readonly IAccountService _accounts;

    //-------------------------------------------------------------------------

    public TransactionService(IRepository<Transaction> transactions,
                              IAccountService accounts)
    {
      _transactions = transactions;
      _accounts = accounts;
    }

    //-------------------------------------------------------------------------

    public void AddTransaction(TransactionDto newTransaction)
    {
      
    }

    //-------------------------------------------------------------------------
  }
}