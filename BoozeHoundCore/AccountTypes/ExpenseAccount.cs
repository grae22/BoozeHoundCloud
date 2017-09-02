using System.Linq;

namespace BoozeHoundCore.AccountTypes
{
  internal class ExpenseAccount : IAccountType
  {
    //-------------------------------------------------------------------------

    public static ExpenseAccount Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new ExpenseAccount();
        }

        return _instance;
      }
    }

    //-------------------------------------------------------------------------

    public string Name { get; } = "Expense";

    private static ExpenseAccount _instance;

    //-------------------------------------------------------------------------

    private ExpenseAccount()
    {
    }

    //-------------------------------------------------------------------------

    public bool CanTransactWith(IAccountType accountType)
    {
      return new IAccountType[]
        {
          BankAccount.Instance
        }
        .Contains(accountType);
    }

    //-------------------------------------------------------------------------
  }
}
