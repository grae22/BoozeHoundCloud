using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;
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
    private Mock<IAccountTypeService> _accountTypeService;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _accounts = new Mock<IRepository<Account>>();
      _accountTypes = new Mock<IRepository<AccountType>>();
      _accountTypeService = new Mock<IAccountTypeService>();

      _testObject =
        new AccountService(
          _accounts.Object,
          _accountTypes.Object,
          _accountTypeService.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnInstantiateServiceWithNullAccountRepository()
    {
      try
      {
        new AccountService(null, _accountTypes.Object, _accountTypeService.Object);
      }
      catch (Exception ex)
      {
        StringAssert.Contains("null", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnInstantiateServiceWithNullAccountTypeRepository()
    {
      try
      {
        new AccountService(_accounts.Object, null, _accountTypeService.Object);
      }
      catch (Exception ex)
      {
        StringAssert.Contains("null", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("GetAccount")]
    public void ReturnsNullOnNotFound()
    {
      Account account = _testObject.GetAccount(123);

      Assert.Null(account);
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("GetAccount")]
    public void GetAccount()
    {
      _accounts.Setup(x => x.Get(123)).Returns(new Account());

      Account account = _testObject.GetAccount(123);

      Assert.NotNull(account);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddAccount")]
    public void ExceptionIfNameAlreadyExists()
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
        _accounts.Verify(x => x.Add(It.IsAny<Account>()), Times.Never);
        _accounts.Verify(x => x.Save(), Times.Never);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddAccount")]
    public void ExceptionIfTypeNotFound()
    {
      var newAccount = new AccountDto
      {
        Name = "New Account",
        AccountTypeId = 1
      };

      // Null account object will be returned for any search so we don't get
      // "account already exists" error.
      _accounts.Setup(x => x.Get(It.IsAny<Expression<Func<Account, bool>>>()))
        .Returns<Account>(null);

      try
      {
        _testObject.AddAccount(newAccount);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("not found", ex.Message);
        _accounts.Verify(x => x.Add(It.IsAny<Account>()), Times.Never);
        _accounts.Verify(x => x.Save(), Times.Never);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddAccount")]
    public void AccountRepositoryAddAndSaveCalled()
    {
      var newAccount = new AccountDto
      {
        Name = "New Account",
        AccountTypeId = 1
      };

      // Null account object will be returned for any search so we don't get
      // "account already exists" error.
      _accounts.Setup(x => x.Get(It.IsAny<Expression<Func<Account, bool>>>()))
        .Returns<Account>(null);

      // AccountType will be returned for account's type.
      _accountTypes.Setup(x => x.Get(newAccount.AccountTypeId))
        .Returns(new AccountType());

      _testObject.AddAccount(newAccount);

      _accounts.Verify(x => x.Add(It.IsAny<Account>()), Times.Once);
      _accounts.Verify(x => x.Save(), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("PerformTransfer")]
    public void DebitAccountIsNotNull()
    {
      try
      {
        _testObject.PerformTransfer(null, new Account(), 1.23m);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("Debit account cannot be null.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("PerformTransfer")]
    public void CreditAccountIsNotNull()
    {
      try
      {
        _testObject.PerformTransfer(new Account(), null, 1.23m);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("Credit account cannot be null.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("PerformTransfer")]
    [TestCase(0)]
    [TestCase(-1)]
    public void AmountIsNonZeroAndPositive(decimal amount)
    {
      try
      {
        _testObject.PerformTransfer(new Account(), new Account(), amount);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("PerformTransfer")]
    public void DebitAccountBalanceUpdated()
    {
      AllowTransfersBetweenAnyAccountTypes();

      var account = new Account
      {
        Balance = 0m
      };

      _testObject.PerformTransfer(account, new Account(), 1.23m);

      Assert.AreEqual(-1.23m, account.Balance);
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("PerformTransfer")]
    public void CreditAccountBalanceUpdated()
    {
      AllowTransfersBetweenAnyAccountTypes();

      var account = new Account
      {
        Balance = 0m
      };

      _testObject.PerformTransfer(new Account(), account, 1.23m);

      Assert.AreEqual(1.23m, account.Balance);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("PerformTransfer")]
    public void ExceptionWhenTransferBetweenAccountTypesNotAllowed()
    {
      var fromAccount = new Account
      {
        AccountType = new AccountType { Id = 1, Name = "A1" }
      };

      var toAccount = new Account
      {
        AccountType = new AccountType { Id = 2, Name = "A2" }
      };

      try
      {
        _testObject.PerformTransfer(fromAccount, toAccount, 1.23m);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains($"Transfer not allowed from account type '{fromAccount.AccountType.Name}' to type '{toAccount.AccountType.Name}'.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //=========================================================================

    private void AllowTransfersBetweenAnyAccountTypes()
    {
      _accountTypeService.Setup(x =>
          x.IsTransferAllowed(It.IsAny<AccountType>(), It.IsAny<AccountType>()))
        .Returns(true);
    }

    //-------------------------------------------------------------------------
  }
}
