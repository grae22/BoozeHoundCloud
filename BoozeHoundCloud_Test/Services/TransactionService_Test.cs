using BoozeHoundCloud;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models.Core;
using BoozeHoundCloud.Services;

namespace BoozeHoundCloud_Test.Services
{
  [TestFixture]
  [Category("TransactionService")]
  internal class TransactionService_Test
  {
    //-------------------------------------------------------------------------

    private TransactionService _testObject;
    private Mock<IRepository<Transaction>> _transactions;
    private Mock<IAccountService> _accounts;

    //-------------------------------------------------------------------------
    
    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _transactions = new Mock<IRepository<Transaction>>();
      _accounts = new Mock<IAccountService>();
      _testObject = new TransactionService(_transactions.Object, _accounts.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void AddTransaction()
    {
      var transaction = new TransactionDto();

      _testObject.AddTransaction(transaction);

      _transactions.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Once);
      _transactions.Verify(x => x.Save(), Times.Once);
    }

    //-------------------------------------------------------------------------
  }
}
