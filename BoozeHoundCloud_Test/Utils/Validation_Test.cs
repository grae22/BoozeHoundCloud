using System;
using NUnit.Framework;
using BoozeHoundCloud.Utils;

namespace BoozeHoundCloud_Test.Utils
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
      catch (ArgumentException ex)
      {
        Assert.AreEqual("Value must be non-zero and positive.", ex.Message);
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
      catch (ArgumentException ex)
      {
        Assert.AreEqual("Value must be non-zero and positive.", ex.Message);
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
  }
}
