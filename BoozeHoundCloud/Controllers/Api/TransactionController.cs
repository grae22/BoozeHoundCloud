using System.Web.Http;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;
using BoozeHoundCloud.Services;

namespace BoozeHoundCloud.Controllers.Api
{
  public class TransactionController : ApiController
  {
    //-------------------------------------------------------------------------

    private ITransactionService _transactions;

    //-------------------------------------------------------------------------

    public TransactionController()
    {
      var context = new ApplicationDbContext();
      var transactionRepository = new GenericRepository<Transaction>(context);
      var accountRepository = new GenericRepository<Account>(context);
      var accountTypeRepository = new GenericRepository<AccountType>(context);
      var accountService = new AccountService(accountRepository, accountTypeRepository);

      _transactions =
        new TransactionService(
          context,
          transactionRepository,
          accountService);
    }

    //-------------------------------------------------------------------------

    public TransactionController(ITransactionService transactions)
    {
      _transactions = transactions;
    }

    //-------------------------------------------------------------------------
  }
}
