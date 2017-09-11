using System;
using System.Net.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud;
using BoozeHoundCloud.Controllers.Api;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Services;

namespace BoozeHoundCloud_Test.Controllers.Api
{
  [TestFixture]
  [Category("TransactionController")]
  internal class TransactionController_Test
  {
    //-------------------------------------------------------------------------

    private TransactionController _testObject;
    private Mock<ITransactionService> _transactionService;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _transactionService = new Mock<ITransactionService>();

      _testObject = new TransactionController(_transactionService.Object);

      _testObject.Request = new HttpRequestMessage(new HttpMethod("POST"), new Uri("http://localhost"));
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void ServiceAddTransactionCalled()
    {
      var transactionDto = new TransactionDto();

      _testObject.AddTransaction(transactionDto);

      _transactionService.Verify(x => x.AddTransaction(transactionDto), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void CreatedResponseReturned()
    {
      var response = _testObject.AddTransaction(new TransactionDto());

      Assert.IsInstanceOf<CreatedNegotiatedContentResult<int>>(response);
    }

    //-------------------------------------------------------------------------
  }
}
