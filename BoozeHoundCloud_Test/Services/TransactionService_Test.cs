using System;
using BoozeHoundCloud;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.DataTransferObjects;
using BoozeHoundCloud.Models;
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
    private Mock<IApplicationDbContext> _context;
    private Mock<IRepository<Transaction>> _transactions;
    private Mock<IAccountService> _accountService;

    //-------------------------------------------------------------------------
    
    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _context = new Mock<IApplicationDbContext>();
      _transactions = new Mock<IRepository<Transaction>>();
      _accountService = new Mock<IAccountService>();

      _testObject =
        new TransactionService(
          _context.Object,
          _transactions.Object,
          _accountService.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionWhenInstantiatedWithNullContext()
    {
      try
      {
        new TransactionService(null, _transactions.Object, _accountService.Object);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("context", ex.Message.ToLower());
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionWhenInstantiatedWithNullTransactionRepository()
    {
      try
      {
        new TransactionService(_context.Object, null, _accountService.Object);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("transaction repository", ex.Message.ToLower());
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionWhenInstantiatedWithNull()
    {
      try
      {
        new TransactionService(_context.Object, _transactions.Object, null);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains("account service", ex.Message.ToLower());
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void ContextSaveCalledOnSuccess()
    {
      // Valid accounts will always be returned so we don't exceptions for unknown
      // debit & credit accounts.
      _accountService.Setup(x => x.GetAccount(It.IsAny<int>())).Returns(new Account());

      _testObject.AddTransaction(new TransactionDto());

      _transactions.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Once);
      _context.Verify(x => x.SaveChanges(), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void ExceptionIfDebitAccountNotFound()
    {
      var transaction = new TransactionDto
      {
        DebitAccountId = 1,
        CreditAccountId = 2
      };

      // Debit account will not be found.
      _accountService.Setup(x => x.GetAccount(transaction.DebitAccountId)).Returns<Account>(null);

      // Credit account will be found.
      _accountService.Setup(x => x.GetAccount(transaction.CreditAccountId)).Returns(new Account());

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
    [Category("AddTransaction")]
    public void ExceptionIfCreditAccountNotFound()
    {
      var transaction = new TransactionDto
      {
        DebitAccountId = 1,
        CreditAccountId = 2
      };

      // Debit account will be found.
      _accountService.Setup(x => x.GetAccount(transaction.DebitAccountId)).Returns(new Account());

      // Credit account will not be found.
      _accountService.Setup(x => x.GetAccount(transaction.CreditAccountId)).Returns<Account>(null);

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

    [Test]
    [Category("AddTransaction")]
    public void DebitAccountDebited()
    {
      var transaction = new TransactionDto
      {
        DebitAccountId = 1,
        CreditAccountId = 2,
        Value = 1.23m
      };

      var debitAccount = new Mock<Account>();

      _accountService.Setup(x => x.GetAccount(transaction.DebitAccountId)).Returns(debitAccount.Object);
      _accountService.Setup(x => x.GetAccount(transaction.CreditAccountId)).Returns(new Account());

      _testObject.AddTransaction(transaction);

      _accountService.Verify(x => x.ApplyDebit(debitAccount.Object, transaction.Value), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void CreditAccountCredited()
    {
      var transaction = new TransactionDto
      {
        DebitAccountId = 1,
        CreditAccountId = 2,
        Value = 1.23m
      };

      var creditAccount = new Mock<Account>();

      _accountService.Setup(x => x.GetAccount(transaction.DebitAccountId)).Returns(new Account());
      _accountService.Setup(x => x.GetAccount(transaction.CreditAccountId)).Returns(creditAccount.Object);

      _testObject.AddTransaction(transaction);

      _accountService.Verify(x => x.ApplyCredit(creditAccount.Object, transaction.Value), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void CreatedTimestampSetCorrectly()
    {
      // Set up so the transaction object added to the repository is saved for us to inspect.
      Transaction transaction = null;

      _transactions.Setup(x => x.Add(It.IsAny<Transaction>()))
        .Callback<Transaction>(t => transaction = t);

      // Set up accounts the transaction will use so we don't get invalid account exceptions.
      _accountService.Setup(x => x.GetAccount(1)).Returns(new Account());
      _accountService.Setup(x => x.GetAccount(2)).Returns(new Account());

      // Creat the transaction dto.
      var transactionDto = new TransactionDto
      {
        DebitAccountId = 1,
        CreditAccountId = 2
      };

      // Add a transaction and inspect the transaction object that gets created.
      _testObject.AddTransaction(transactionDto);

      var justNow = new TimeSpan(0, 0, 2);
      TimeSpan timestampAge = (DateTime.UtcNow - transaction.CreatedTimestamp);

      Assert.True(timestampAge < justNow);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void ProcessedTimestampIsNull()
    {
      // Set up so the transaction object added to the repository is saved for us to inspect.
      Transaction transaction = null;

      _transactions.Setup(x => x.Add(It.IsAny<Transaction>()))
        .Callback<Transaction>(t => transaction = t);

      // Set up accounts the transaction will use so we don't get invalid account exceptions.
      _accountService.Setup(x => x.GetAccount(1)).Returns(new Account());
      _accountService.Setup(x => x.GetAccount(2)).Returns(new Account());

      // Creat the transaction dto.
      var transactionDto = new TransactionDto
      {
        DebitAccountId = 1,
        CreditAccountId = 2
      };

      // Add a transaction and inspect the transaction object that gets created.
      _testObject.AddTransaction(transactionDto);

      Assert.Null(transaction.ProcessedTimestamp);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetTransaction")]
    public void ExistingTransactionReturned()
    {
      _transactions.Setup(x => x.Get(123)).Returns(new Transaction());

      Transaction transaction = _testObject.GetTransaction(123);

      Assert.NotNull(transaction);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("GetTransaction")]
    public void NullReturnedForTransactionThatDoesntExist()
    {
      _transactions.Setup(x => x.Get(123)).Returns<Transaction>(null);

      Transaction transaction = _testObject.GetTransaction(123);

      Assert.Null(transaction);
    }

    //-------------------------------------------------------------------------
  }
}
