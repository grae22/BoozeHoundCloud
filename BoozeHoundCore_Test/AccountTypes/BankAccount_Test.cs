using NUnit.Framework;
using BoozeHoundCore.AccountTypes;

namespace BoozeHoundCore_Test.AccountTypes
{
  [TestFixture]
  [Category("BankAccount")]
  internal class BankAccount_Test
  {
    //-------------------------------------------------------------------------

    private BankAccount _testObject;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      _testObject = BankAccount.Instance;
    }

    //-------------------------------------------------------------------------

    [Test]
    public void Name()
    {
      Assert.AreEqual("Bank", _testObject.Name);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CanTransactWithExpenseAccount()
    {
      Assert.True(_testObject.CanTransactWith(ExpenseAccount.Instance));
    }

    //-------------------------------------------------------------------------
  }
}
