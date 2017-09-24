using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public interface IAccountTypeService
  {
    //-------------------------------------------------------------------------

    bool IsTransferAllowed(AccountType from, AccountType to);
    AccountType Get(int id);

    //-------------------------------------------------------------------------
  }
}
