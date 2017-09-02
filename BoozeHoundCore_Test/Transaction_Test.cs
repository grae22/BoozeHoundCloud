using System;
using NUnit.Framework;
using Moq;
using BoozeHoundCore;

namespace BoozeHoundCore_Test
{
  [TestFixture]
  [Category("Transaction")]
  internal class Transaction_Test
  {
    //-------------------------------------------------------------------------

    private Transaction _testObject;
    private decimal _value;
    private Mock<IAccount> _debitAccount;
    private Mock<IAccount> _creditAccount;
    private string _reference;
    private string _description;
    private DateTime _date;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      _value = 123.45m;
      _debitAccount = new Mock<IAccount>();
      _creditAccount = new Mock<IAccount>();
      _reference = "TestReference";
      _description = "TestDescription";
      _date = new DateTime(2017, 9, 2);  

      _testObject = new Transaction(
        _value,
        _debitAccount.Object,
        _creditAccount.Object,
        _reference,
        _description,
        _date);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnZeroValue()
    {
      try
      {
        new Transaction(
          0m,
          null,
          null,
          null,
          null,
          DateTime.UtcNow);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnNegativeValue()
    {
      try
      {
        new Transaction(
          -1m,
          null,
          null,
          null,
          null,
          DateTime.UtcNow);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnNullDebitAccount()
    {
      try
      {
        new Transaction(
          1m,
          null,
          null,
          null,
          null,
          DateTime.UtcNow);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ExceptionOnNullCreditAccount()
    {
      try
      {
        new Transaction(
          1m,
          _debitAccount.Object,
          null,
          null,
          null,
          DateTime.UtcNow);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ConstructorParams()
    {
      _testObject = new Transaction(
        _value,
        _debitAccount.Object,
        _creditAccount.Object,
        _reference,
        _description,
        _date,
        DateTime.UtcNow);

      Assert.AreEqual(_value, _testObject.Value);
      Assert.AreSame(_debitAccount.Object, _testObject.DebitAccount);
      Assert.AreSame(_creditAccount.Object, _testObject.CreditAccount);
      Assert.AreEqual(_reference, _testObject.Reference);
      Assert.AreEqual(_description, _testObject.Description);
      Assert.AreEqual(_date, _testObject.Date);
      Assert.True(_testObject.IsProcessed);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CreatedTimestampRecordsObjectCreation()
    {
      var fiveSeconds = new TimeSpan(0, 0, 0, 5);

      TimeSpan timeSinceObjectInstantiated = (DateTime.UtcNow - _testObject.CreatedTimestamp);

      Assert.Less(timeSinceObjectInstantiated, fiveSeconds);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ProcessedFlagIsInitiallyFalse()
    {
      Assert.False(_testObject.IsProcessed);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void DebitAccountCorrectlyDebitedOnProcess()
    {
      _testObject.Process();

      _debitAccount.Verify(x => x.ApplyDebit(_value), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void DebitAccountNotCreditedOnProcess()
    {
      _testObject.Process();

      _debitAccount.Verify(x => x.ApplyCredit(It.IsAny<decimal>()), Times.Never);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CreditAccountCorrectlyCreditedOnProcess()
    {
      _testObject.Process();

      _creditAccount.Verify(x => x.ApplyCredit(_value), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CreditAccountNotDebitedOnProcess()
    {
      _testObject.Process();

      _creditAccount.Verify(x => x.ApplyDebit(It.IsAny<decimal>()), Times.Never);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ProcessedFlagIsTrueAfterProcessCalled()
    {
      _testObject.Process();

      Assert.True(_testObject.IsProcessed);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void DoNotProcessMultipleTimes()
    {
      _testObject.Process();
      _testObject.Process();

      _debitAccount.Verify(x => x.ApplyDebit(It.IsAny<decimal>()), Times.Once);
      _creditAccount.Verify(x => x.ApplyCredit(It.IsAny<decimal>()), Times.Once);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ProcessedTimestampIsNullBeforeProcessed()
    {
      Assert.Null(_testObject.ProcessedTimestamp);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ProcessedTimestampRecordsWhenTransactionProcessed()
    {
      _testObject.Process();

      var twoSeconds = new TimeSpan(0, 0, 0, 2);
      TimeSpan timeSinceProcessed = (DateTime.UtcNow - (DateTime)_testObject.ProcessedTimestamp);

      Assert.Less(timeSinceProcessed, twoSeconds);
    }

    //-------------------------------------------------------------------------
  }
}
