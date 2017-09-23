using System;
using System.Linq;
using AutoMapper;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Exceptions;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  internal class TransactionService : ITransactionService
  {
    //-------------------------------------------------------------------------

    private readonly IApplicationDbContext _context;
    private readonly IRepository<Transaction> _transactions;
    private readonly IAccountService _accounts;

    //-------------------------------------------------------------------------

    public static TransactionService Create(IApplicationDbContext context)
    {
      return new TransactionService(
        context,
        new GenericRepository<Transaction>(context),
        AccountService.Create(context));
    }

    //-------------------------------------------------------------------------

    public TransactionService(IApplicationDbContext context,
                              IRepository<Transaction> transactions,
                              IAccountService accounts)
    {
      if (context == null)
      {
        throw new ArgumentException("Context cannot be null.", nameof(context));
      }

      if (transactions == null)
      {
        throw new ArgumentException("Transaction repository cannot be null.", nameof(transactions));
      }

      if (accounts == null)
      {
        throw new ArgumentException("Account service cannot be null", nameof(accounts));
      }

      _context = context;
      _transactions = transactions;
      _accounts = accounts;
    }

    //-------------------------------------------------------------------------

    public int AddTransaction(TransactionDto newTransaction)
    {
      var transaction = Mapper.Map<TransactionDto, Transaction>(newTransaction);

      SetCreatedTimestamp(transaction);
      SetProcessedTimestampToNull(transaction);
      ResolveAccounts(newTransaction, transaction);
      UpdateAccountBalances(transaction);
      AddTransactionAndSave(transaction);

      return transaction.Id;
    }

    //-------------------------------------------------------------------------

    public void UpdateTransaction(Transaction modifiedTransaction)
    {
      Transaction orignalTransaction = _transactions.Get(modifiedTransaction.Id);

      if (orignalTransaction == null)
      {
        throw new ArgumentException($"Transaction not found with id {modifiedTransaction.Id}.");
      }

      if (modifiedTransaction.Value != orignalTransaction.Value)
      {
        throw new BusinessLogicException("Transaction value cannot change.");
      }

      if (modifiedTransaction.DebitAccountId != orignalTransaction.DebitAccountId)
      {
        throw new BusinessLogicException("Transaction debit-account id cannot change.");
      }

      if (modifiedTransaction.CreditAccountId != orignalTransaction.CreditAccountId)
      {
        throw new BusinessLogicException("Transaction credit-account id cannot change.");
      }

      if (modifiedTransaction.Date != orignalTransaction.Date)
      {
        throw new BusinessLogicException("Transaction date cannot change.");
      }

      if ((modifiedTransaction.CreatedTimestamp - orignalTransaction.CreatedTimestamp).Ticks == 0)
      {
        throw new BusinessLogicException("Transaction created-timestamp cannot change.");
      }

      if (orignalTransaction.ProcessedTimestamp != null &&
          modifiedTransaction.ProcessedTimestamp.Equals(orignalTransaction.ProcessedTimestamp) == false)
      {
        throw new BusinessLogicException("Transaction processed-timestamp cannot change.");
      }

      orignalTransaction.Reference = modifiedTransaction.Reference;
      orignalTransaction.Description = modifiedTransaction.Description;

      _transactions.Update(orignalTransaction);
      _transactions.Save();
    }

    //-------------------------------------------------------------------------

    public IQueryable<Transaction> GetAll()
    {
      return _transactions.Get();
    }

    //-------------------------------------------------------------------------

    public Transaction GetTransaction(int id)
    {
      return _transactions.Get(id);
    }

    //-------------------------------------------------------------------------

    private static void SetCreatedTimestamp(Transaction transaction)
    {
      transaction.CreatedTimestamp = DateTime.UtcNow;
    }

    //-------------------------------------------------------------------------

    private static void SetProcessedTimestampToNull(Transaction transaction)
    {
      transaction.ProcessedTimestamp = null;
    }

    //-------------------------------------------------------------------------

    private void ResolveAccounts(TransactionDto transactionDto,
                                 Transaction transaction)
    {
      transaction.DebitAccount = _accounts.GetAccount(transactionDto.DebitAccountId);

      if (transaction.DebitAccount == null)
      {
        throw new ArgumentException(
          $"No account found for debit account id {transactionDto.DebitAccountId}.",
          nameof(transactionDto.DebitAccountId));
      }

      transaction.CreditAccount = _accounts.GetAccount(transactionDto.CreditAccountId);

      if (transaction.CreditAccount == null)
      {
        throw new ArgumentException(
          $"No account found for credit account id {transactionDto.CreditAccountId}.",
          nameof(transactionDto.CreditAccountId));
      }
    }

    //-------------------------------------------------------------------------

    private void UpdateAccountBalances(Transaction transaction)
    {
      _accounts.PerformTransfer(
        transaction.DebitAccount,
        transaction.CreditAccount,
        transaction.Value);
    }

    //-------------------------------------------------------------------------

    private void AddTransactionAndSave(Transaction transaction)
    {
      _transactions.Add(transaction);
      _context.SaveChanges();
    }

    //-------------------------------------------------------------------------
  }
}