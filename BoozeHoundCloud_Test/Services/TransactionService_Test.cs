using NUnit.Framework;
using Moq;
using BoozeHoundCloud.DataAccess;
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
      _transactions = new Mock<IRepository<Transaction>>();
      _accounts = new Mock<IAccountService>();
      _testObject = new TransactionService(_transactions.Object, _accounts.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void AddTransaction()
    {
    }

    //-------------------------------------------------------------------------
  }
}
