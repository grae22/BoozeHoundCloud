using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Areas.Core.Exceptions;
using BoozeHoundCloud.Areas.Core.Models;
using BoozeHoundCloud.Areas.Core.Services;

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

      _testObject.AddTransaction(new Transaction());

      _transactions.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Once);
      _context.Verify(x => x.SaveChanges(), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void ExceptionIfDebitAccountNotFound()
    {
      var transaction = new Transaction
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
      var transaction = new Transaction
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
    public void AccountServicePerformTransferCalled()
    {
      var transaction = new Transaction
      {
        DebitAccountId = 1,
        CreditAccountId = 2,
        Value = 1.23m
      };

      var debitAccount = new Mock<Account>();
      var creditAccount = new Mock<Account>();

      _accountService.Setup(x => x.GetAccount(transaction.DebitAccountId)).Returns(debitAccount.Object);
      _accountService.Setup(x => x.GetAccount(transaction.CreditAccountId)).Returns(creditAccount.Object);

      _testObject.AddTransaction(transaction);

      _accountService.Verify(x =>
        x.PerformTransfer(debitAccount.Object, creditAccount.Object, transaction.Value), Times.Once);
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("AddTransaction")]
    public void CreatedTimestampSetCorrectly()
    {
      // Creat the transaction.
      var transaction = new Transaction
      {
        DebitAccountId = 1,
        CreditAccountId = 2
      };

      // Set up so the transaction object added to the repository is saved for us to inspect.
      _transactions.Setup(x => x.Add(It.IsAny<Transaction>()))
        .Callback<Transaction>(t => transaction = t);

      // Set up accounts the transaction will use so we don't get invalid account exceptions.
      _accountService.Setup(x => x.GetAccount(1)).Returns(new Account());
      _accountService.Setup(x => x.GetAccount(2)).Returns(new Account());

      // Add a transaction and inspect the transaction object that gets created.
      _testObject.AddTransaction(transaction);

      var justNow = new TimeSpan(0, 0, 2);
      TimeSpan timestampAge = (DateTime.UtcNow - transaction.CreatedTimestamp);

      Assert.True(timestampAge < justNow);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AddTransaction")]
    public void ProcessedTimestampIsNull()
    {
      // Creat the transaction.
      var transaction = new Transaction
      {
        DebitAccountId = 1,
        CreditAccountId = 2
      };

      // Set up so the transaction object added to the repository is saved for us to inspect.
      _transactions.Setup(x => x.Add(It.IsAny<Transaction>()))
        .Callback<Transaction>(t => transaction = t);

      // Set up accounts the transaction will use so we don't get invalid account exceptions.
      _accountService.Setup(x => x.GetAccount(1)).Returns(new Account());
      _accountService.Setup(x => x.GetAccount(2)).Returns(new Account());

      // Add a transaction and inspect the transaction object that gets created.
      _testObject.AddTransaction(transaction);

      Assert.Null(transaction.ProcessedTimestamp);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("UpdateTransaction")]
    public void UpdateAndSaveCalledOnRepo()
    {
      var transaction = new Transaction
      {
        Id = 123
      };

      _transactions.Setup(x => x.Get(transaction.Id)).Returns(transaction);

      _testObject.UpdateTransaction(transaction);

      _transactions.Verify(x => x.Update(transaction), Times.Once);
      _transactions.Verify(x => x.Save(), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("UpdateTransaction")]
    public void ExceptionIfOriginalTransactionNotFound()
    {
      var transaction = new Transaction
      {
        Id = 123
      };

      _transactions.Setup(x => x.Get(transaction.Id)).Returns<Transaction>(null);

      try
      {
        _testObject.UpdateTransaction(transaction);
      }
      catch (ArgumentException ex)
      {
        StringAssert.Contains(transaction.Id.ToString(), ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("UpdateTransaction")]
    public void AllowedUpdateFieldsAreUpdated()
    {
      var original = new Transaction
      {
        Id = 123,
        Reference = "TestReference",
        Description = "TestDescription"
      };

      var modified = new Transaction
      {
        Id = original.Id,
        Reference = "NewTestReference",
        Description = "NewTestDescription"
      };

      _transactions.Setup(x => x.Get(original.Id)).Returns(original);

      _testObject.UpdateTransaction(modified);

      Assert.AreEqual(modified.Reference, original.Reference);
      Assert.AreEqual(modified.Description, original.Description);
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("UpdateTransaction")]
    public void BusinessLogicExceptionOnValueChange()
    {
      var original = new Transaction
      {
        Id = 123,
        Value = 1.23m
      };

      var modified = new Transaction
      {
        Id = original.Id,
        Value = 1.24m
      };

      _transactions.Setup(x => x.Get(original.Id)).Returns(original);

      try
      {
        _testObject.UpdateTransaction(modified);
      }
      catch (BusinessLogicException ex)
      {
        Assert.AreEqual("Transaction value cannot change.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("UpdateTransaction")]
    public void BusinessLogicExceptionOnDebitAccountChange()
    {
      var original = new Transaction
      {
        Id = 123,
        DebitAccountId = 1
      };

      var modified = new Transaction
      {
        Id = original.Id,
        DebitAccountId = 2
      };

      _transactions.Setup(x => x.Get(original.Id)).Returns(original);

      try
      {
        _testObject.UpdateTransaction(modified);
      }
      catch (BusinessLogicException ex)
      {
        Assert.AreEqual("Transaction debit-account id cannot change.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("UpdateTransaction")]
    public void BusinessLogicExceptionOnCreditAccountChange()
    {
      var original = new Transaction
      {
        Id = 123,
        CreditAccountId = 1
      };

      var modified = new Transaction
      {
        Id = original.Id,
        CreditAccountId = 2
      };

      _transactions.Setup(x => x.Get(original.Id)).Returns(original);

      try
      {
        _testObject.UpdateTransaction(modified);
      }
      catch (BusinessLogicException ex)
      {
        Assert.AreEqual("Transaction credit-account id cannot change.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("UpdateTransaction")]
    public void BusinessLogicExceptionOnDateChange()
    {
      var original = new Transaction
      {
        Id = 123,
        Date = DateTime.UtcNow
      };

      var modified = new Transaction
      {
        Id = original.Id,
        Date = DateTime.UtcNow + new TimeSpan(1, 0, 0)
      };

      _transactions.Setup(x => x.Get(original.Id)).Returns(original);

      try
      {
        _testObject.UpdateTransaction(modified);
      }
      catch (BusinessLogicException ex)
      {
        Assert.AreEqual("Transaction date cannot change.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("UpdateTransaction")]
    public void BusinessLogicExceptionOnCreatedTimestampChange()
    {
      var original = new Transaction
      {
        Id = 123,
        CreatedTimestamp = DateTime.UtcNow
      };

      var modified = new Transaction
      {
        Id = original.Id,
        CreatedTimestamp = DateTime.UtcNow + new TimeSpan(1, 0, 0)
      };

      _transactions.Setup(x => x.Get(original.Id)).Returns(original);

      try
      {
        _testObject.UpdateTransaction(modified);
      }
      catch (BusinessLogicException ex)
      {
        Assert.AreEqual("Transaction created-timestamp cannot change.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("UpdateTransaction")]
    public void BusinessLogicExceptionOnProcessedTimestampChange()
    {
      var original = new Transaction
      {
        Id = 123,
        ProcessedTimestamp = DateTime.UtcNow
      };

      var modified = new Transaction
      {
        Id = original.Id,
        ProcessedTimestamp = DateTime.UtcNow + new TimeSpan(1, 0, 0)
      };

      _transactions.Setup(x => x.Get(original.Id)).Returns(original);

      try
      {
        _testObject.UpdateTransaction(modified);
      }
      catch (BusinessLogicException ex)
      {
        Assert.AreEqual("Transaction processed-timestamp cannot change.", ex.Message);
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------
    
    [Test]
    [Category("UpdateTransaction")]
    public void NoBusinessLogicExceptionOnTransactionChangeToProcessed()
    {
      var original = new Transaction
      {
        Id = 123,
        ProcessedTimestamp = null
      };

      var modified = new Transaction
      {
        Id = original.Id,
        ProcessedTimestamp = DateTime.UtcNow
      };

      _transactions.Setup(x => x.Get(original.Id)).Returns(original);

      try
      {
        _testObject.UpdateTransaction(modified);
      }
      catch (BusinessLogicException)
      {
        Assert.Fail();
      }

      Assert.Pass();
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

    [Test]
    [Category("GetAllTransactions")]
    public void AllAccountsReturned()
    {
      _transactions.Setup(x => x.Get()).Returns(
        new List<Transaction>
        {
          new Transaction(),
          new Transaction(),
          new Transaction()
        }.AsQueryable());

      IQueryable<Transaction> result = _testObject.GetAll();

      Assert.AreEqual(3, result.Count());
    }

    //-------------------------------------------------------------------------
  }
}
