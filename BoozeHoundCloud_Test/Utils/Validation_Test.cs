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
    [TestCase(0)]
    [TestCase(-1)]
    public void ExceptionWhenZero(decimal amount)
    {
      try
      {
        Validation.ValueIsNonZeroAndPositive(amount);
      }
      catch (ArgumentException ex)
      {
        Assert.AreEqual($"Value must be non-zero and positive, was {amount:N2}.", ex.Message);
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
