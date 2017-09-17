using System;
using System.Linq;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.DataAccess;

namespace BoozeHoundCloud.Areas.Core.Services
{
  internal class AccountTypeService : IAccountTypeService
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
        _allowedTransferMappings
          .Get()
          .ToList()
          .Any(mapping =>
            mapping.DebitAccountType == from &&
            mapping.CreditAccountType == to);

      return mappingExists;
    }

    //-------------------------------------------------------------------------
  }
}