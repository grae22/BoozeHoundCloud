using System;
using System.Linq.Expressions;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud.Services;

namespace BoozeHoundCloud_Test.Services
{
  [TestFixture]
  [Category("AccountService")]
  internal class AccountService_Test
  {
    //-------------------------------------------------------------------------

    private AccountService _testObject;
    private Mock<IRepository<Account>> _accounts;
    private Mock<IRepository<AccountType>> _accountTypes;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      _accounts = new Mock<IRepository<Account>>();
      _accountTypes = new Mock<IRepository<AccountType>>();
      _testObject = new AccountService(_accounts.Object, _accountTypes.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void AddAccountExceptionIfNameAlreadyExists()
    {
      // An account object will be returned for any search.
      _accounts.Setup(x => x.Get(It.IsAny<Expression<Func<Account, bool>>>()))
        .Returns(new Account());

      var newAccount = new AccountDto
      {
        Name = "New Account"
      };

      try
      {
        _testObject.AddAccount(newAccount);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("already exists", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void AddAccountExceptionIfTypeNotFound()
    {
      // Null account object will be returned for any search so we don't get
      // "account already exists" error.
      _accounts.Setup(x => x.Get(It.IsAny<Expression<Func<Account, bool>>>()))
        .Returns<Account>(null);

      var newAccount = new AccountDto
      {
        Name = "New Account",
        AccountTypeId = 1
      };

      try
      {
        _testObject.AddAccount(newAccount);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("not found", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------
  }
}
