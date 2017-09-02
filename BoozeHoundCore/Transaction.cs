using System;
using BoozeHoundCore.Utils;

namespace BoozeHoundCore
{
  internal class Transaction : ITransaction
  {
    //-------------------------------------------------------------------------

    public decimal Value { get; private set; }
    public IAccount DebitAccount { get; private set; }
    public IAccount CreditAccount { get; private set; }
    public string Reference { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public DateTime Timestamp { get; private set; }
    public bool IsProcessed { get; private set; }

    //-------------------------------------------------------------------------

    public Transaction(decimal value,
                       IAccount debitAccount,
                       IAccount creditAccount,
                       string reference,
                       string description,
                       DateTime date)
    {
      Validation.ValueIsNonZeroAndPositive(value);
      Validation.AccountNotNull(debitAccount);
      Validation.AccountNotNull(creditAccount);

      Value = value;
      DebitAccount = debitAccount;
      CreditAccount = creditAccount;
      Reference = reference;
      Description = description;
      Date = date;
      Timestamp = DateTime.UtcNow;
    }

    //-------------------------------------------------------------------------

    public void Process()
    {
      
    }

    //-------------------------------------------------------------------------
  }
}
