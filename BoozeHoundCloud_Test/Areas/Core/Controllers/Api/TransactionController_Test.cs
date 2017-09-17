using System;
using System.Net.Http;
using System.Web.Http.Results;
using BoozeHoundCloud;
using BoozeHoundCloud.Areas.Core.Controllers.Api;
using BoozeHoundCloud.Areas.Core.DataTransferObjects;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.Areas.Core.Services;
using Moq;
using NUnit.Framework;

namespace BoozeHoundCloud_Test.Areas.Core.Controllers.Api
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
    
    [Test]
    [Category("GetTransaction")]
    public void OkResultReturnedWhenTransactionFound()
    {
      var transaction = new Transaction { Reference = "T123" };

      _transactionService.Setup(x => x.GetTransaction(123))
        .Returns(transaction);

      var response = _testObject.GetTransaction(123);
      Assert.IsInstanceOf<OkNegotiatedContentResult<TransactionDto>>(response);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetTransaction")]
    public void CorrectTransactionIsReturned()
    {
      var transaction = new Transaction { Reference = "T123" };

      _transactionService.Setup(x => x.GetTransaction(123))
        .Returns(transaction);

      var response = _testObject.GetTransaction(123);

      TransactionDto returnedTransaction = ((OkNegotiatedContentResult<TransactionDto>)response).Content;
      Assert.AreEqual(transaction.Reference, returnedTransaction.Reference);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetTransaction")]
    public void NotFoundResponseIfTransactionNotFound()
    {
      _transactionService.Setup(x => x.GetTransaction(123))
        .Returns<Transaction>(null);

      var response = _testObject.GetTransaction(123);

      Assert.IsInstanceOf<NotFoundResult>(response);
    }

    //-------------------------------------------------------------------------
  }
  }

