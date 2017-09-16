﻿using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Models.Core;
using BoozeHoundCloud.Services;

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
      _interAccountMappings.Setup(x =>
        x.Get(It.IsAny<Expression<Func<InterAccountTypeTransactionMapping, bool>>>()))
          .Returns(new InterAccountTypeTransactionMapping());

      Assert.True(_testObject.IsTransferAllowed(new AccountType(), new AccountType()));
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("IsTransferAllowed")]
    public void TransferNotAllowedForAccountTypesNotMappedInRepo()
    {
      _interAccountMappings.Setup(x =>
          x.Get(It.IsAny<Expression<Func<InterAccountTypeTransactionMapping, bool>>>()))
        .Returns< InterAccountTypeTransactionMapping>(null);

      Assert.False(_testObject.IsTransferAllowed(new AccountType(), new AccountType()));
    }

    //-------------------------------------------------------------------------
  }
}