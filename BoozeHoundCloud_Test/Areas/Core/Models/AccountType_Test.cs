using BoozeHoundCloud.Areas.Core.Models;
using NUnit.Framework;

namespace BoozeHoundCloud_Test.Areas.Core.Models
{
  [TestFixture]
  [Category("AccountType")]
  internal class AccountType_Test
  {
    //-------------------------------------------------------------------------

    private AccountType _testObject;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      _testObject = new AccountType
      {
        Id = 123,
        Name = "TestAccountType"
      };
    }

    //-------------------------------------------------------------------------

    [Test]
    public void AccountTypesWithSameIdAreEqual()
    {
      Assert.AreEqual(new AccountType { Id = 123 }, _testObject);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void AccountTypesWithDifferentIdAreNotEqual()
    {
      Assert.AreNotEqual(new AccountType { Id = 0 }, _testObject);
    }

    //-------------------------------------------------------------------------
  }
}