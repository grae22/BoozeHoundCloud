using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Services
{
  public interface IAccountService
  {
    //-------------------------------------------------------------------------

    Account GetAccount(int id);
    Account AddAccount(AccountDto newAccount);
    void ApplyDebit(Account account, decimal value);
    void ApplyCredit(Account account, decimal value);

    //-------------------------------------------------------------------------
  }
}
