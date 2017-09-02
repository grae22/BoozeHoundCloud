using System;

namespace BoozeHoundCore
{
  internal interface ITransaction
  {
    //-------------------------------------------------------------------------

    decimal Value { get; }
    IAccount DebitAccount { get; }
    IAccount CreditAccount { get; }
    string Reference { get; }
    string Description { get; }
    DateTime Date { get; }
    DateTime CreatedTimestamp { get; }
    bool IsProcessed { get; }
    DateTime? ProcessedTimestamp { get; }

    //-------------------------------------------------------------------------

    void Process();

    //-------------------------------------------------------------------------
  }
}
