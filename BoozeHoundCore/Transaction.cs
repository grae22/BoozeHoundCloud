// TODO: Ability to void the transaction.

using System;
using BoozeHoundCore.Utils;

namespace BoozeHoundCore
{
  internal class Transaction : ITransaction
  {
    //-------------------------------------------------------------------------

    public decimal Value { get; }
    public IAccount DebitAccount { get; }
    public IAccount CreditAccount { get; }
    public string Reference { get; }
    public string Description { get; }
    public DateTime Date { get; }
    public DateTime CreatedTimestamp { get; }
    public bool IsProcessed { get; private set; }
    public DateTime? ProcessedTimestamp { get; private set; }

    //-------------------------------------------------------------------------

    public Transaction(decimal value,
                       IAccount debitAccount,
                       IAccount creditAccount,
                       string reference,
                       string description,
                       DateTime date,
                       bool isProcessed = false,
                       DateTime? processedTimestamp = null)
    {
      Validation.ValueIsNonZeroAndPositive(value);
      Validation.AccountNotNull(debitAccount);
      Validation.AccountNotNull(creditAccount);
      ValidateProcessedParams(isProcessed, processedTimestamp);

      Value = value;
      DebitAccount = debitAccount;
      CreditAccount = creditAccount;
      Reference = reference;
      Description = description;
      Date = date;
      CreatedTimestamp = DateTime.UtcNow;
      IsProcessed = isProcessed;
      ProcessedTimestamp = processedTimestamp;
    }

    //-------------------------------------------------------------------------

    public void Process()
    {
      if (IsProcessed)
      {
        return;
      }

      DebitAccount.ApplyDebit(Value);
      CreditAccount.ApplyCredit(Value);
      MarkAsProcessed();
    }

    //-------------------------------------------------------------------------

    private static void ValidateProcessedParams(bool isProcessed, DateTime? processedTimestamp)
    {
      if (isProcessed &&
          processedTimestamp == null)
      {
        throw new ArgumentException("Processed timestamp cannot be null if transaction is processed.");
      }
    }

    //-------------------------------------------------------------------------

    private void MarkAsProcessed()
    {
      IsProcessed = true;
      ProcessedTimestamp = DateTime.UtcNow;
    }

    //-------------------------------------------------------------------------
  }
}
