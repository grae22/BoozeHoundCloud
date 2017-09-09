using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud;
using BoozeHoundCloud.Controllers.Api;
using BoozeHoundCloud.DAL;
using BoozeHoundCloud.Dtos;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud_Test.Controllers.Api
{
  [TestFixture]
  [Category("AccountController")]
  internal class AccountController_Test
  {
    //-------------------------------------------------------------------------

    private AccountController _testObject;
    private Mock<IRepository<Account>> _accounts;
    private Mock<IRepository<AccountType>> _accountTypes;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _accounts = new Mock<IRepository<Account>>();
      _accountTypes = new Mock<IRepository<AccountType>>();

      _testObject = new AccountController(
        _accounts.Object,
        _accountTypes.Object);

      _testObject.Request = new HttpRequestMessage(new HttpMethod("POST"), new Uri("http://localhost"));
    }

    //-------------------------------------------------------------------------

    [Test]
    public void GetAccountWithId()
    {
      var account = new Mock<Account>();
      account.Object.Name = "AccountName";
      account.Object.AccountTypeId = 456;
      account.Object.Balance = 1.23m;

      _accounts.Setup(x => x.Get(123)).Returns(account.Object);

      var result = _testObject.GetAccount(123);

      Assert.IsInstanceOf<OkNegotiatedContentResult<AccountDto>>(result);

      var resultAccount = result as OkNegotiatedContentResult<AccountDto>;
      
      Assert.AreEqual(account.Object.Name, resultAccount.Content.Name);
      Assert.AreEqual(account.Object.AccountTypeId, resultAccount.Content.AccountTypeId);
      Assert.AreEqual(account.Object.Balance, resultAccount.Content.Balance);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void GetAccountWithIdNotFound()
    {
      _accounts.Setup(x => x.Get(123)).Returns<Account>(null);

      var result = _testObject.GetAccount(123);

      Assert.IsInstanceOf<NotFoundResult>(result);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CreateAccount()
    {
      _accounts.Setup(x => x.Get(It.IsAny<Expression<Func<Account, bool>>>()))
        .Returns<Account>(null);

      _accountTypes.Setup(x => x.Get(1))
        .Returns(new Mock<AccountType>().Object);

      var accountDto = new AccountDto
      {
        Name = "TestAccount",
        AccountTypeId = 1,
        Balance = 1.23m
      };

      var result = _testObject.CreateAccount(accountDto);

      Assert.IsInstanceOf<CreatedNegotiatedContentResult<Account>>(result);

      _accounts.Verify(x => x.Save(), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void BadRequestOnCreateIfAccountAlreadyExists()
    {
      _accounts.Setup(x => x.Get(It.IsAny<Expression<Func<Account, bool>>>()))
        .Returns(new Account());

      var accountDto = new AccountDto
      {
        Name = "TestAccount"
      };

      var result = _testObject.CreateAccount(accountDto);

      Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
      StringAssert.Contains("already exists", ((BadRequestErrorMessageResult)result).Message);

      _accounts.Verify(x => x.Save(), Times.Never);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void BadRequestOnCreateIfAccountTypeNotFound()
    {
      _accounts.Setup(
        x => x.Get(It.IsAny<Expression<Func<Account, bool>>>()))
          .Returns<Account>(null);

      _accountTypes.Setup(
        x => x.Get(It.IsAny<int>()))
          .Returns<AccountType>(null);

      var accountDto = new AccountDto
      {
        AccountTypeId = 1,
      };

      var result = _testObject.CreateAccount(accountDto);

      Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
      StringAssert.Contains("not found", ((BadRequestErrorMessageResult)result).Message);

      _accounts.Verify(x => x.Save(), Times.Never);
    }

    //-------------------------------------------------------------------------
  }
}
