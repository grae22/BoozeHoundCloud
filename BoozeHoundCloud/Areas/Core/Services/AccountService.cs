using System;
using System.Linq;
using BoozeHoundCloud.Areas.Core.Exceptions;
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
      var accountTypes = new GenericRepository<AccountType>(context);

      return new AccountService(
        new GenericRepository<Account>(context),
        accountTypes,
        new AccountTypeService(accountTypes, allowedTransferMappings));
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

    public void AddAccount(Account account)
    {
      Account existingAccount = GetAccount(account.Name);

      if (existingAccount != null)
      {
        throw new ArgumentException(
          $"Account already exists with name '{account.Name}'.",
          nameof(account.Name));
      }

      AccountType accountType = _accountTypes.Get(account.AccountTypeId);

      if (accountType == null)
      {
        throw new ArgumentException(
          $"AccountType not found for id {account.AccountTypeId}.",
          nameof(account.AccountTypeId));
      }

      _accounts.Add(account);
      _accounts.Save();
    }

    //-------------------------------------------------------------------------

    public void UpdateAccount(Account modifiedAccount)
    {
      Account originalAccount = _accounts.Get(modifiedAccount.Id);

      if (originalAccount.AccountTypeId != modifiedAccount.AccountTypeId)
      {
        throw new BusinessLogicException("AccountType cannot change.");
      }

      if (originalAccount.Balance != modifiedAccount.Balance)
      {
        throw new BusinessLogicException("Balance cannot change.");
      }

      bool hasNameChanged = !modifiedAccount.Name.Equals(originalAccount.Name);
      if (hasNameChanged)
      {
        var isNameInUseByAnotherAccount =
          _accounts
            .Get()
            .Any(a => 
              a.Id != originalAccount.Id &&
              a.Name.Equals(modifiedAccount.Name, StringComparison.OrdinalIgnoreCase));

        if (isNameInUseByAnotherAccount)
        {
          throw new BusinessLogicException($"Account name '{modifiedAccount.Name}' is already in use.");
        }
      }

      originalAccount.Name = modifiedAccount.Name;

      _accounts.Update(originalAccount);
      _accounts.Save();
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