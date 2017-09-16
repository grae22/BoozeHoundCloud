using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Services
{
  public interface IAccountTypeService
  {
    //-------------------------------------------------------------------------

    bool IsTransferAllowed(AccountType from, AccountType to);

    //-------------------------------------------------------------------------
  }
}
