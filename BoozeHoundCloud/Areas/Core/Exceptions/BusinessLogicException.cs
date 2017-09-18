using System;

namespace BoozeHoundCloud.Areas.Core.Exceptions
{
  public class BusinessLogicException : Exception
  {
    //-------------------------------------------------------------------------

    public BusinessLogicException(string message)
    :
      base(message)
    {
    }

    //-------------------------------------------------------------------------
  }
}