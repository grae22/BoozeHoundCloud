using System;
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
      // Valid accounts will always be returned so we don't exceptions for unknown
      // debit & credit accounts.
      _accounts.Setup(x => x.GetAccount(It.IsAny<int>())).Returns(new Account());

      _testObject.AddTransaction(new TransactionDto());

      _transactions.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Once);
      _transactions.Verify(x => x.Save(), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnAddTransactionIfDebitAccountNotFound()
    {
      var transaction = new TransactionDto
      {
        DebitAccountId = 1,
        CreditAccountId = 2
      };

      // Debit account will not be found.
      _accounts.Setup(x => x.GetAccount(transaction.DebitAccountId)).Returns<Account>(null);

      // Credit account will be found.
      _accounts.Setup(x => x.GetAccount(transaction.CreditAccountId)).Returns(new Account());

      try
      {
        _testObject.AddTransaction(transaction);
      }
      catch (ArgumentException ex)
      {
        _transactions.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Never);
        _transactions.Verify(x => x.Save(), Times.Never);

        StringAssert.Contains($"No account found for debit account id {transaction.DebitAccountId}.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnAddTransactionIfCreditAccountNotFound()
    {
      var transaction = new TransactionDto
      {
        DebitAccountId = 1,
        CreditAccountId = 2
      };

      // Debit account will be found.
      _accounts.Setup(x => x.GetAccount(transaction.DebitAccountId)).Returns(new Account());

      // Credit account will not be found.
      _accounts.Setup(x => x.GetAccount(transaction.CreditAccountId)).Returns<Account>(null);

      try
      {
        _testObject.AddTransaction(transaction);
      }
      catch (ArgumentException ex)
      {
        _transactions.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Never);
        _transactions.Verify(x => x.Save(), Times.Never);

        StringAssert.Contains($"No account found for credit account id {transaction.CreditAccountId}.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------
  }
  }
