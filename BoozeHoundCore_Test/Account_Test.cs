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
    private Mock<IAccountType> _accountType;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      _accountType = new Mock<IAccountType>();
      _testObject = new Account("TestObject", _accountType.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ConstructorParams()
    {
      Assert.AreEqual("TestObject", _testObject.Name);
      Assert.AreSame(_accountType.Object, _testObject.AccountType);
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

    [Test]
    public void DebitAndCredit()
    {
      _testObject.ApplyDebit(10.15m);
      _testObject.ApplyCredit(10.15m);

      Assert.AreEqual(0m, _testObject.Balance);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void ToStringValue()
    {
      _accountType.SetupGet(x => x.Name).Returns("TestAccountType");

      Assert.AreEqual(
        $"Account: [Name=\"{_testObject.Name}\", Type=\"{_accountType.Object.Name}\", Balance={0:N2}]",
        _testObject.ToString());
    }

    //-------------------------------------------------------------------------
  }
}
