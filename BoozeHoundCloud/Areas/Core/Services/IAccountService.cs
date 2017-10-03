using System;
using System.Linq;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public interface IAccountService
  {
    //-------------------------------------------------------------------------

    IQueryable<Account> GetAll(Guid userId);
    Account GetAccount(int id);
    void AddAccount(Account account);
    void UpdateAccount(Account account);
    void PerformTransfer(Account debitAccount, Account creditAccount, decimal amount);

    //-------------------------------------------------------------------------
  }
}
