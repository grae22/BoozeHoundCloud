using System;
using System.Linq;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  internal class AccountTypeService : IAccountTypeService
  {
    //-------------------------------------------------------------------------

    private readonly IRepository<AccountType> _accountTypes;
    private readonly IRepository<InterAccountTypeTransactionMapping> _allowedTransferMappings;

    //-------------------------------------------------------------------------

    public static AccountTypeService Create(IApplicationDbContext context)
    {
      var accountTypes = new GenericRepository<AccountType>(context);
      var allowedTransferMappings = new GenericRepository<InterAccountTypeTransactionMapping>(context);
      return new AccountTypeService(accountTypes, allowedTransferMappings);
    }

    //-------------------------------------------------------------------------

    public AccountTypeService(IRepository<AccountType> accountTypes,
                              IRepository<InterAccountTypeTransactionMapping> allowedTransferMappings)
    {
      if (accountTypes == null)
      {
        throw new ArgumentException("Account types repository cannot be null.", nameof(accountTypes));
      }

      if (allowedTransferMappings == null)
      {
        throw new ArgumentException("Allowed transfer mappings repository cannot be null.", nameof(allowedTransferMappings));
      }

      _accountTypes = accountTypes;
      _allowedTransferMappings = allowedTransferMappings;
    }

    //-------------------------------------------------------------------------

    public bool IsTransferAllowed(AccountType from, AccountType to)
    {
      bool mappingExists =
        _allowedTransferMappings
          .Get()
          .ToList()
          .Any(mapping =>
            mapping.DebitAccountType == from &&
            mapping.CreditAccountType == to);

      return mappingExists;
    }

    //-------------------------------------------------------------------------

    public AccountType Get(int id)
    {
      return _accountTypes.Get(id);
    }

    //-------------------------------------------------------------------------
  }
}