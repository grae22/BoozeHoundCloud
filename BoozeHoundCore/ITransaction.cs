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
    DateTime Timestamp { get; }
    bool IsProcessed { get; }

    //-------------------------------------------------------------------------

    void Process();

    //-------------------------------------------------------------------------
  }
}
