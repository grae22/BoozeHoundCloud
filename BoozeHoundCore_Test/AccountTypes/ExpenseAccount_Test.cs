using NUnit.Framework;
using BoozeHoundCore.AccountTypes;

namespace BoozeHoundCore_Test.AccountTypes
{
  [TestFixture]
  [Category("ExpenseAccount")]
  internal class ExpenseAccount_Test
  {
    //-------------------------------------------------------------------------

    private ExpenseAccount _testObject;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      _testObject = ExpenseAccount.Instance;
    }

    //-------------------------------------------------------------------------

    [Test]
    public void Name()
    {
      Assert.AreEqual("Expense", _testObject.Name);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void CanTransactWithBankAccount()
    {
      Assert.True(_testObject.CanTransactWith(BankAccount.Instance));
    }

    //-------------------------------------------------------------------------
  }
}
