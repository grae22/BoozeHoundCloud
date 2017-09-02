namespace BoozeHoundCore
{
  internal interface IAccountType
  {
    //-------------------------------------------------------------------------

    string Name { get; }

    //-------------------------------------------------------------------------

    bool CanTransactWith(IAccountType accountType);

    //-------------------------------------------------------------------------
  }
}
