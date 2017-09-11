using System;
using System.Web.Http;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;
using BoozeHoundCloud.Services;

namespace BoozeHoundCloud.Controllers.Api
{
  public class TransactionController : ApiController
  {
    //-------------------------------------------------------------------------

    private readonly ITransactionService _transactionService;

    //-------------------------------------------------------------------------

    public TransactionController()
    {
      var context = new ApplicationDbContext();
      var transactionRepository = new GenericRepository<Transaction>(context);
      var accountRepository = new GenericRepository<Account>(context);
      var accountTypeRepository = new GenericRepository<AccountType>(context);
      var accountService = new AccountService(accountRepository, accountTypeRepository);

      _transactionService =
        new TransactionService(
          context,
          transactionRepository,
          accountService);
    }

    //-------------------------------------------------------------------------

    public TransactionController(ITransactionService transactionService)
    {
      _transactionService = transactionService;
    }

    //-------------------------------------------------------------------------

    public IHttpActionResult AddTransaction(TransactionDto transactionDto)
    {
      int id = _transactionService.AddTransaction(transactionDto);

      return Created(
        new Uri($"{Request.RequestUri}/{id}"),
        id);
    }

    //-------------------------------------------------------------------------
  }
}
