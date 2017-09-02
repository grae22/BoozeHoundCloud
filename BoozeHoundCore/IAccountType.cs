namespace BoozeHoundCore
{
  public interface IAccountType
  {
    //-------------------------------------------------------------------------

    string Name { get; }

    //-------------------------------------------------------------------------

    bool CanTransactWith(IAccountType accountType);

    //-------------------------------------------------------------------------
  }
}
