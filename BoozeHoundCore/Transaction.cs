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
    public bool IsProcessed => (ProcessedTimestamp != null);
    public DateTime? ProcessedTimestamp { get; private set; }

    //-------------------------------------------------------------------------

    public Transaction(decimal value,
                       IAccount debitAccount,
                       IAccount creditAccount,
                       string reference,
                       string description,
                       DateTime date,
                       DateTime? createdTimestamp = null,
                       DateTime? processedTimestamp = null)
    {
      Validation.ValueIsNonZeroAndPositive(value);
      Validation.AccountNotNull(debitAccount);
      Validation.AccountNotNull(creditAccount);

      Value = value;
      DebitAccount = debitAccount;
      CreditAccount = creditAccount;
      Reference = reference ?? "";
      Description = description ?? "";
      Date = date;
      CreatedTimestamp = createdTimestamp ?? DateTime.UtcNow;
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

    public override string ToString()
    {
      string processedTimestamp =
        (ProcessedTimestamp == null ? "(null)" : $"{ProcessedTimestamp:yyyy-MM-dd HH:mm:ss}");

      return
        "Transaction: [" +
          $"Value={Value:N2}, " +
          $"Debit=\"{DebitAccount.Name}\", " +
          $"Credit=\"{CreditAccount.Name}\", " +
          $"Reference=\"{Reference}\", " +
          $"Description=\"{Description}\", " +
          $"Date=\"{Date:yyyy-MM-dd}\", " +
          $"Created=\"{CreatedTimestamp:yyyy-MM-dd HH:mm:ss}\", " +
          $"Processed=\"{processedTimestamp}\"]";
    }

    //-------------------------------------------------------------------------
    
    private void MarkAsProcessed()
    {
      ProcessedTimestamp = DateTime.UtcNow;
    }

    //-------------------------------------------------------------------------
  }
}
