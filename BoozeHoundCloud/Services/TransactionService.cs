using System;
using AutoMapper;
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
      var transaction = Mapper.Map<TransactionDto, Transaction>(newTransaction);

      ResolveAccounts(newTransaction, transaction);

      _transactions.Add(transaction);
      _transactions.Save();
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
  }
}