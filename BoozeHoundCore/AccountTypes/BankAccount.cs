using System.Linq;

namespace BoozeHoundCore.AccountTypes
{
  public class BankAccount : IAccountType
  {
    //-------------------------------------------------------------------------

    public static BankAccount Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new BankAccount();
        }

        return _instance;
      }
    }

    //-------------------------------------------------------------------------

    public string Name { get; } = "Bank";

    private static BankAccount _instance;

    //-------------------------------------------------------------------------

    private BankAccount()
    {
    }

    //-------------------------------------------------------------------------

    public bool CanTransactWith(IAccountType accountType)
    {
      return new IAccountType[]
        {
          ExpenseAccount.Instance
        }
        .Contains(accountType);
    }

    //-------------------------------------------------------------------------
  }
}
