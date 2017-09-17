using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.Areas.Core.Services;

namespace BoozeHoundCloud_TestServices
{
  [TestFixture]
  [Category("AccountTypeService")]
  internal class AccountTypeService_Test
  {
    //-------------------------------------------------------------------------

    private AccountTypeService _testObject;
    private Mock<IRepository<InterAccountTypeTransactionMapping>> _interAccountMappings;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      _interAccountMappings = new Mock<IRepository<InterAccountTypeTransactionMapping>>();
      _testObject = new AccountTypeService(_interAccountMappings.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnNullAccountTypeRepository()
    {
      try
      {
        new AccountTypeService(null);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("Allowed transfer mappings repository cannot be null.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("IsTransferAllowed")]
    public void TransferIsAllowedForAccountTypesMappedInRepo()
    {
      // Set up the mock so that a inter account type transfer mapping will be returned,
      // in other words - any account can transfer to any other account.
      var debitAccountType = new AccountType { Id = 1 };
      var creditAccountType = new AccountType { Id = 2 };

      var debitAccount = new Account { AccountType = debitAccountType };
      var creditAccount = new Account { AccountType = creditAccountType };

      _interAccountMappings.Setup(x => x.Get())
        .Returns(
          new[]
          {
            new InterAccountTypeTransactionMapping
            {
              DebitAccountType = debitAccountType,
              CreditAccountType = creditAccountType
            }
          });

      Assert.True(_testObject.IsTransferAllowed(debitAccount.AccountType, creditAccount.AccountType));
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("IsTransferAllowed")]
    public void TransferNotAllowedForAccountTypesNotMappedInRepo()
    {
      _interAccountMappings.Setup(x => x.Get())
        .Returns(new List<InterAccountTypeTransactionMapping>());

      Assert.False(_testObject.IsTransferAllowed(new AccountType(), new AccountType()));
    }

    //-------------------------------------------------------------------------
  }
}