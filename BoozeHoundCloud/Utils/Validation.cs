using System;

namespace BoozeHoundCloud.Utils
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

      throw new ArgumentException("Value must be non-zero and positive.");
    }

    //-------------------------------------------------------------------------
  }
}
