using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud;
using BoozeHoundCloud.Areas.Core.Controllers.Api;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.Areas.Core.Services;

namespace BoozeHoundCloud_Test.Areas.Core.Controllers.Api
{
  [TestFixture]
  [Category("AccountController")]
  internal class AccountController_Test
  {
    //-------------------------------------------------------------------------

    private AccountController _testObject;
    private Mock<IAccountService> _accountService;
    private Mock<IAccountTypeService> _accountTypeService;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _accountService = new Mock<IAccountService>();
      _accountTypeService = new Mock<IAccountTypeService>();

      _testObject = new AccountController(_accountService.Object, _accountTypeService.Object);

      _testObject.Request = new HttpRequestMessage(new HttpMethod("POST"), new Uri("http://localhost"));
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetAllAccounts")]
    public void QueryableJsonResultsReturned()
    {
      var response = _testObject.GetAll();

      Assert.IsInstanceOf<JsonResult<IQueryable<Account>>>(response);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetAllAccounts")]
    public void AccountsOfTypeQueryableJsonResultsReturned()
    {
      var response = _testObject.GetAll(0);

      Assert.IsInstanceOf<JsonResult<IQueryable<Account>>>(response);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetAllAccounts")]
    public void OnlyAccountsOfSpecifiedTypeReturned()
    {
      _accountService.Setup(x => x.GetAll())
        .Returns(
          new[]
          {
            new Account { AccountType = new AccountType { Id = 123 } },
            new Account { AccountType = new AccountType { Id = 0 } },
            new Account { AccountType = new AccountType { Id = 123 } }
          }
          .AsQueryable());

      var response = (JsonResult<IQueryable<Account>>)_testObject.GetAll(123);

      Assert.AreEqual(2, response.Content.Count());
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetAccount")]
    public void AccountPropertiesCorrect()
    {
      var account = new Mock<Account>();
      account.Object.Name = "AccountName";
      account.Object.AccountTypeId = 456;
      account.Object.Balance = 1.23m;

      _accountService.Setup(x => x.GetAccount(123)).Returns(account.Object);

      var result = _testObject.GetAccount(123);

      Assert.IsInstanceOf<OkNegotiatedContentResult<AccountDto>>(result);

      var resultAccount = result as OkNegotiatedContentResult<AccountDto>;
      
      Assert.AreEqual(account.Object.Name, resultAccount.Content.Name);
      Assert.AreEqual(account.Object.AccountTypeId, resultAccount.Content.AccountTypeId);
      Assert.AreEqual(account.Object.Balance, resultAccount.Content.Balance);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetAccount")]
    public void NotFoundResultIfNotFound()
    {
      _accountService.Setup(x => x.GetAccount(123)).Returns<Account>(null);

      var result = _testObject.GetAccount(123);

      Assert.IsInstanceOf<NotFoundResult>(result);
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("CreateAccount")]
    public void ServiceAddAccountCalled()
    {
      _accountService.Setup(x => x.AddAccount(It.IsAny<AccountDto>()))
        .Returns(new Account());

      var result = _testObject.CreateAccount(new AccountDto());

      Assert.IsInstanceOf<CreatedNegotiatedContentResult<Account>>(result);

      _accountService.Verify(x => x.AddAccount(It.IsAny<AccountDto>()), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("CreateAccount")]
    public void BadRequestOnAccountServiceReturnsNullAccount()
    {
      var result = _testObject.CreateAccount(new AccountDto());

      Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("CreateAccount")]
    public void BadRequestOnAccountServiceException()
    {
      _accountService.Setup(x => x.AddAccount(It.IsAny<AccountDto>()))
        .Throws<ArgumentException>();

      var result = _testObject.CreateAccount(new AccountDto());

      Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetAccountsAccountCanCredit")]
    public void NotFoundIfFromAccountNotFound()
    {
      var response = _testObject.GetAccountsAccountCanCredit(123);

      Assert.IsInstanceOf<NotFoundResult>(response);
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("GetAccountsAccountCanCredit")]
    public void AllowedToAccountsReturned()
    {
      _accountService.Setup(x => x.GetAccount(123)).Returns(new Account());
      _accountService.Setup(x => x.GetAll()).Returns(new List<Account> { new Account(), new Account() }.AsQueryable);
      _accountTypeService.Setup(x => x.IsTransferAllowed(It.IsAny<AccountType>(), It.IsAny<AccountType>())).Returns(true);

      var response = (JsonResult<List<Account>>)_testObject.GetAccountsAccountCanCredit(123);

      Assert.AreEqual(2, response.Content.Count);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetAccountsAccountCanDebit")]
    public void NotFoundIfToAccountNotFound()
    {
      var response = _testObject.GetAccountsAccountCanDebit(123);

      Assert.IsInstanceOf<NotFoundResult>(response);
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("GetAccountsAccountCanDebit")]
    public void AllowedFromAccountsReturned()
    {
      _accountService.Setup(x => x.GetAccount(123)).Returns(new Account());
      _accountService.Setup(x => x.GetAll()).Returns(new List<Account> { new Account(), new Account() }.AsQueryable);
      _accountTypeService.Setup(x => x.IsTransferAllowed(It.IsAny<AccountType>(), It.IsAny<AccountType>())).Returns(true);

      var response = (JsonResult<List<Account>>)_testObject.GetAccountsAccountCanDebit(123);

      Assert.AreEqual(2, response.Content.Count);
    }

    //-------------------------------------------------------------------------
  }
}
