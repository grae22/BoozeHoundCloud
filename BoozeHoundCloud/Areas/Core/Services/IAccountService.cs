using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public interface IAccountService
  {
    //-------------------------------------------------------------------------

    Account GetAccount(int id);
    Account AddAccount(AccountDto newAccount);
    void PerformTransfer(Account debitAccount, Account creditAccount, decimal amount);

    //-------------------------------------------------------------------------
  }
}
