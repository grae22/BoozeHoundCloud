using System;
using System.Collections.Generic;
using System.Linq;
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
    private Mock<IRepository<AccountType>> _accountTypes;
    private Mock<IRepository<InterAccountTypeTransactionMapping>> _interAccountMappings;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      _accountTypes = new Mock<IRepository<AccountType>>();
      _interAccountMappings = new Mock<IRepository<InterAccountTypeTransactionMapping>>();
      _testObject = new AccountTypeService(_accountTypes.Object, _interAccountMappings.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnNullAccountTypeRepository()
    {
      try
      {
        new AccountTypeService(null, _interAccountMappings.Object);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("Account types repository cannot be null.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnNullMappingsRepository()
    {
      try
      {
        new AccountTypeService(_accountTypes.Object, null);
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
          }.AsQueryable());

      Assert.True(_testObject.IsTransferAllowed(debitAccount.AccountType, creditAccount.AccountType));
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("IsTransferAllowed")]
    public void TransferNotAllowedForAccountTypesNotMappedInRepo()
    {
      _interAccountMappings.Setup(x => x.Get())
        .Returns(new List<InterAccountTypeTransactionMapping>().AsQueryable());

      Assert.False(_testObject.IsTransferAllowed(new AccountType(), new AccountType()));
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetAccountType")]
    public void NullReturnedIfNotFound()
    {
      AccountType accountType = _testObject.Get(123);

      Assert.Null(accountType);
    }

    //-------------------------------------------------------------------------
  }
}