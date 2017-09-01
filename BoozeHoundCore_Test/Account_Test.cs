using System;
using NUnit.Framework;
using Moq;
using BoozeHoundCore;

namespace BoozeHoundCore_Test
{
  [TestFixture]
  [Category("Account")]
  internal class Account_Test
  {
    //-------------------------------------------------------------------------

    private Account _testObject;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      var accountType = new Mock<IAccountType>();

      _testObject = new Account("TestObject", accountType.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void InitialBalanceIsZero()
    {
      Assert.AreEqual(0d, _testObject.Balance);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ApplyDebit()
    {
      _testObject.ApplyDebit(10);

      Assert.AreEqual(-10, _testObject.Balance);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ArgumentExceptionOnApplyDebitWithZeroValue()
    {
      try
      {
        _testObject.ApplyDebit(0);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ArgumentExceptionOnApplyDebitWithNegativeValue()
    {
      try
      {
        _testObject.ApplyDebit(-1);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ApplyCredit()
    {
      _testObject.ApplyCredit(10);

      Assert.AreEqual(10, _testObject.Balance);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ArgumentExceptionOnApplyCreditWithZeroValue()
    {
      try
      {
        _testObject.ApplyDebit(0);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ArgumentExceptionOnApplyCreditWithNegativeValue()
    {
      try
      {
        _testObject.ApplyDebit(-1);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------
  }
}
