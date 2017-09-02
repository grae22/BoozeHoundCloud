using System;

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
      ValidateValueIsNonZeroAndPositive(value);

      Balance -= value;
    }

    //-------------------------------------------------------------------------

    public void ApplyCredit(decimal value)
    {
      ValidateValueIsNonZeroAndPositive(value);

      Balance += value;
    }

    //-------------------------------------------------------------------------

    public override string ToString()
    {
      return $"Account: [Name=\"{Name}\", Type=\"{AccountType.Name}\", Balance={Balance:N2}]";
    }

    //-------------------------------------------------------------------------

    private static void ValidateValueIsNonZeroAndPositive(decimal value)
    {
      if (value > 0)
      {
        return;
      }

      throw new ArgumentException("Value must be positive & non-zero.");
    }

    //-------------------------------------------------------------------------
  }
}
