using System;
using System.Linq;
using AutoMapper;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Utils;

namespace BoozeHoundCloud.Areas.Core.Services
{
  internal class AccountService : IAccountService
  {
    //-------------------------------------------------------------------------

    private readonly IRepository<Account> _accounts;
    private readonly IRepository<AccountType> _accountTypes;
    private readonly IAccountTypeService _accountTypeService;

    //-------------------------------------------------------------------------

    public static AccountService Create(IApplicationDbContext context)
    {
      var allowedTransferMappings = new GenericRepository<InterAccountTypeTransactionMapping>(context);

      return new AccountService(
        new GenericRepository<Account>(context),
        new GenericRepository<AccountType>(context),
        new AccountTypeService(allowedTransferMappings));
    }

    //-------------------------------------------------------------------------

    public AccountService(IRepository<Account> accounts,
                          IRepository<AccountType> accountTypes,
                          IAccountTypeService accountTypeService)
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
      _accountTypeService = accountTypeService;
    }

    //-------------------------------------------------------------------------

    public IQueryable<Account> GetAll()
    {
      return _accounts.Get();
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

    public void PerformTransfer(Account debitAccount,
                                Account creditAccount,
                                decimal amount)
    {
      if (debitAccount == null)
      {
        throw new ArgumentException("Debit account cannot be null.", nameof(debitAccount));
      }

      if (creditAccount == null)
      {
        throw new ArgumentException("Credit account cannot be null.", nameof(creditAccount));
      }

      Validation.ValueIsNonZeroAndPositive(amount);

      ValidateTransferBetweenAccountsIsAllowed(debitAccount, creditAccount);
      UpdateAccountBalances(debitAccount, creditAccount, amount);
    }
    
    //-------------------------------------------------------------------------

    private Account GetAccount(string name)
    {
      return _accounts.Get(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    
    //-------------------------------------------------------------------------

    private void ValidateTransferBetweenAccountsIsAllowed(Account debitAccount, Account creditAccount)
    {
      bool isTransferBetweenAccountTypesAllowed =
        _accountTypeService.IsTransferAllowed(debitAccount.AccountType, creditAccount.AccountType);

      if (isTransferBetweenAccountTypesAllowed == false)
      {
        throw new ArgumentException(
          $"Transfer not allowed from account type '{debitAccount.AccountType.Name}' to type '{creditAccount.AccountType.Name}'.");
      }
    }

    //-------------------------------------------------------------------------

    private static void UpdateAccountBalances(Account debitAccount,
                                              Account creditAccount,
                                              decimal amount)
    {
      debitAccount.Balance -= amount;
      creditAccount.Balance += amount;
    }

    //-------------------------------------------------------------------------
  }
}