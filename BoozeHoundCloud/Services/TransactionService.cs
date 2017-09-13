﻿using System;
using AutoMapper;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Services
{
  internal class TransactionService : ITransactionService
  {
    //-------------------------------------------------------------------------

    private readonly IApplicationDbContext _context;
    private readonly IRepository<Transaction> _transactions;
    private readonly IAccountService _accounts;

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
      ApplyDebitAndCreditToAccounts(transaction);
      AddTransactionAndSave(transaction);

      return transaction.Id;
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

    private void ApplyDebitAndCreditToAccounts(Transaction transaction)
    {
      _accounts.ApplyDebit(transaction.DebitAccount, transaction.Value);
      _accounts.ApplyCredit(transaction.CreditAccount, transaction.Value);
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