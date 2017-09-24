using System;
using System.Web.Http;
using AutoMapper;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.Areas.Core.Services;

namespace BoozeHoundCloud.Areas.Core.Controllers.Api
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
      var interAccountMappings = new GenericRepository<InterAccountTypeTransactionMapping>(context);
      var accountTypeService = new AccountTypeService(interAccountMappings);
      var accountService = new AccountService(accountRepository, accountTypeRepository, accountTypeService);

      _transactionService =
        new TransactionService(
          context,
          transactionRepository,
          accountService);
    }

    //-------------------------------------------------------------------------

    // Constructor provided for unit-testing, could be removed if dependency injection is used.

    internal TransactionController(ITransactionService transactionService)
    {
      _transactionService = transactionService;
    }

    //-------------------------------------------------------------------------

    [HttpPut]
    public IHttpActionResult AddTransaction(TransactionDto transactionDto)
    {
      var transaction = Mapper.Map<TransactionDto, Transaction>(transactionDto);

      int id = _transactionService.AddTransaction(transaction);

      return Created(
        new Uri($"{Request.RequestUri}/{id}"),
        id);
    }

    //-------------------------------------------------------------------------

    [HttpGet]
    public IHttpActionResult GetTransaction(int id)
    {
      Transaction transaction = _transactionService.GetTransaction(id);

      if (transaction == null)
      {
        return NotFound();
      }

      var transactionDto = Mapper.Map<Transaction, TransactionDto>(transaction);

      return Ok(transactionDto);
    }

    //-------------------------------------------------------------------------

    [HttpGet]
    public IHttpActionResult GetAll()
    {
      return Json(_transactionService.GetAll());
    }

    //-------------------------------------------------------------------------
  }
}
