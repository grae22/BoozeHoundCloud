using System;

namespace BoozeHoundCore.Utils
{
  internal class Validation
  {
    //-------------------------------------------------------------------------

    public static void ValueIsNonZeroAndPositive(decimal value)
    {
      if (value > 0m)
      {
        return;
      }

      throw new ArgumentException("Value must non-zero and positive.");
    }

    //-------------------------------------------------------------------------

    public static void AccountNotNull(IAccount account)
    {
      if (account != null)
      {
        return;
      }

      throw new ArgumentException("Account cannot be null.");
    }

    //-------------------------------------------------------------------------
  }
}
