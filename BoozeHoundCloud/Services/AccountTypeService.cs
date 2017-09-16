using System;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Services
{
  internal class AccountTypeService
  {
    //-------------------------------------------------------------------------

    private readonly IRepository<InterAccountTypeTransactionMapping> _allowedTransferMappings;

    //-------------------------------------------------------------------------

    public AccountTypeService(IRepository<InterAccountTypeTransactionMapping> allowedTransferMappings)
    {
      if (allowedTransferMappings == null)
      {
        throw new ArgumentException("Allowed transfer mappings repository cannot be null.", nameof(allowedTransferMappings));
      }

      _allowedTransferMappings = allowedTransferMappings;
    }

    //-------------------------------------------------------------------------

    public bool IsTransferAllowed(AccountType from, AccountType to)
    {
      bool mappingExists =
        _allowedTransferMappings.Get(mapping =>
          mapping.DebitAccountType == from &&
          mapping.CreditAccountType == to) != null;

      return mappingExists;
    }

    //-------------------------------------------------------------------------
  }
}