using System;
using AutoMapper;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Services
{
  internal class AccountService : IAccountService
  {
    //-------------------------------------------------------------------------

    private readonly IRepository<Account> _accounts;
    private readonly IRepository<AccountType> _accountTypes;

    //-------------------------------------------------------------------------

    public AccountService(IRepository<Account> accounts,
                          IRepository<AccountType> accountTypes)
    {
      if (accounts == null)
      {
        throw new ArgumentException("Accounts object cannot be null.", nameof(accounts));
      }

      if (accountTypes == null)
      {
        throw new ArgumentException("AccountTypes object cannot be null.", nameof(accountTypes));
      }

      _accounts = accounts;
      _accountTypes = accountTypes;
    }

    //-------------------------------------------------------------------------

    public Account GetAccount(int id)
    {
      return _accounts.Get(id);
    }

    //-------------------------------------------------------------------------

    public Account AddAccount(AccountDto newAccount)
    {
      Account existingAccount = GetAccount(newAccount.Name);

      if (existingAccount != null)
      {
        throw new ArgumentException(
          $"Account already exists with name '{newAccount.Name}'.",
          nameof(newAccount.Name));
      }

      AccountType accountType = _accountTypes.Get(newAccount.AccountTypeId);

      if (accountType == null)
      {
        throw new ArgumentException(
          $"AccountType not found for id {newAccount.AccountTypeId}.",
          nameof(newAccount.AccountTypeId));
      }

      var createdAccount = Mapper.Map<AccountDto, Account>(newAccount);

      _accounts.Add(createdAccount);
      _accounts.Save();

      return createdAccount;
    }

    //-------------------------------------------------------------------------

    private Account GetAccount(string name)
    {
      return _accounts.Get(
        a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    //-------------------------------------------------------------------------
  }
}