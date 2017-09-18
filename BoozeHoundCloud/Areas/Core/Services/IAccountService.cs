using System.Linq;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public interface IAccountService
  {
    //-------------------------------------------------------------------------

    IQueryable<Account> GetAll();
    Account GetAccount(int id);
    Account AddAccount(AccountDto newAccount);
    void UpdateAccount(Account account);
    void PerformTransfer(Account debitAccount, Account creditAccount, decimal amount);

    //-------------------------------------------------------------------------
  }
}
