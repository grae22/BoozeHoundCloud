using System;
using NUnit.Framework;
using Moq;
using BoozeHoundCore;
using BoozeHoundCore.Utils;

namespace BoozeHoundCore_Test.Utils
{
  [TestFixture]
  [Category("Validation")]
  internal class Validation_Test
  {
    //-------------------------------------------------------------------------

    [Test]
    [Category("NonZeroAndPositive")]
    public void ExceptionWhenZero()
    {
      try
      {
        Validation.ValueIsNonZeroAndPositive(0m);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("NonZeroAndPositive")]
    public void ExceptionWhenNegative()
    {
      try
      {
        Validation.ValueIsNonZeroAndPositive(-1m);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("NonZeroAndPositive")]
    public void NoExceptionWhenNonZeroAndPositive()
    {
      try
      {
        Validation.ValueIsNonZeroAndPositive(1m);
      }
      catch (ArgumentException)
      {
        Assert.Fail();
      }

      Assert.Pass();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AccountNotNull")]
    public void ExceptionWhenAccountIsNull()
    {
      try
      {
        Validation.AccountNotNull(null);
      }
      catch (ArgumentException)
      {
        Assert.Pass();
      }

      Assert.Fail();
    }

    //-------------------------------------------------------------------------

    [Test]
    [Category("AccountNotNull")]
    public void NoExceptionWhenAccountIsNotNull()
    {
      var account = new Mock<IAccount>();

      try
      {
        Validation.AccountNotNull(account.Object);
      }
      catch (ArgumentException)
      {
        Assert.Fail();
      }

      Assert.Pass();
    }

    //-------------------------------------------------------------------------
  }
}
