using System;
using System.Net.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud;
using BoozeHoundCloud.Controllers.Api;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;
using BoozeHoundCloud.Services;

namespace BoozeHoundCloud_Test.Controllers.Api
{
  [TestFixture]
  [Category("AccountController")]
  internal class AccountController_Test
  {
    //-------------------------------------------------------------------------

    private AccountController _testObject;
    private Mock<IAccountService> _accounts;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _accounts = new Mock<IAccountService>();

      _testObject = new AccountController(_accounts.Object);

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

      _accounts.Setup(x => x.GetAccount(123)).Returns(account.Object);

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
      _accounts.Setup(x => x.GetAccount(123)).Returns<Account>(null);

      var result = _testObject.GetAccount(123);

      Assert.IsInstanceOf<NotFoundResult>(result);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CreateAccount()
    {
      _accounts.Setup(x => x.AddAccount(It.IsAny<AccountDto>()))
        .Returns(new Account());

      var result = _testObject.CreateAccount(new AccountDto());

      Assert.IsInstanceOf<CreatedNegotiatedContentResult<Account>>(result);

      _accounts.Verify(x => x.AddAccount(It.IsAny<AccountDto>()), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CreateAccountBadRequestOnAccountServiceReturnsNullAccount()
    {
      var result = _testObject.CreateAccount(new AccountDto());

      Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CreateAccountBadRequestOnAccountServiceException()
    {
      _accounts.Setup(x => x.AddAccount(It.IsAny<AccountDto>()))
        .Throws<ArgumentException>();

      var result = _testObject.CreateAccount(new AccountDto());

      Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
    }

    //-------------------------------------------------------------------------
  }
}
