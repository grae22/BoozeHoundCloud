using System;
using System.Net.Http;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud;
using BoozeHoundCloud.Controllers.Api;
using BoozeHoundCloud.Services;

namespace BoozeHoundCloud_Test.Controllers.Api
{
  [TestFixture]
  [Category("TransactionController")]
  internal class TransactionController_Test
  {
    //-------------------------------------------------------------------------

    private TransactionController _testObject;
    private Mock<ITransactionService> _transactions;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _transactions = new Mock<ITransactionService>();

      _testObject = new TransactionController(_transactions.Object);

      _testObject.Request = new HttpRequestMessage(new HttpMethod("POST"), new Uri("http://localhost"));
    }

    //-------------------------------------------------------------------------
  }
}
