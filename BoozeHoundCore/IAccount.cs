namespace BoozeHoundCore
{
  internal interface IAccount
  {
    //-------------------------------------------------------------------------

    string Name { get; }
    IAccountType AccountType { get; }
    decimal Balance { get; }

    //-------------------------------------------------------------------------

    void ApplyDebit(decimal value);
    void ApplyCredit(decimal value);

    //-------------------------------------------------------------------------
  }
}
