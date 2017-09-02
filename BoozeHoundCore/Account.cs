using BoozeHoundCore.Utils;

namespace BoozeHoundCore
{
  internal class Account : IAccount
  {
    //-------------------------------------------------------------------------

    public string Name { get; }
    public IAccountType AccountType { get; }
    public decimal Balance { get; private set; }

    //-------------------------------------------------------------------------

    public Account(string name, IAccountType accountType)
    {
      Name = name;
      AccountType = accountType;
    }

    //-------------------------------------------------------------------------

    public void ApplyDebit(decimal value)
    {
      Validation.ValueIsNonZeroAndPositive(value);

      Balance -= value;
    }

    //-------------------------------------------------------------------------

    public void ApplyCredit(decimal value)
    {
      Validation.ValueIsNonZeroAndPositive(value);

      Balance += value;
    }

    //-------------------------------------------------------------------------

    public override string ToString()
    {
      return $"Account: [Name=\"{Name}\", Type=\"{AccountType.Name}\", Balance={Balance:n2}]";
    }

    //-------------------------------------------------------------------------
  }
}
