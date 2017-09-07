using System.Data.Entity;
using BoozeHoundCloud;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud.Controllers.Api;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud_Test.Controllers.Api
{
  [TestFixture]
  [Category("AccountController")]
  internal class AccountController_Test
  {
    //-------------------------------------------------------------------------

    private AccountController _testObject;
    private Mock<IApplicationDbContext> _context;
    private Mock<IDbSet<Account>> _accounts;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      AutoMapperConfig.Initialise();

      _context = new Mock<IApplicationDbContext>
      {
        CallBase = false
      };

      _accounts = new Mock<IDbSet<Account>>();
      _context.Object.Accounts = _accounts.Object;
        
      _testObject = new AccountController(_context.Object);
    }

    //-------------------------------------------------------------------------

    [Test]
    public void GetAccountWithId()
    {
      var account = new Mock<Account>();
      _accounts.Setup(x => x.Find(123)).Returns(account.Object);

      var result = _testObject.GetAccount(123);
    }

    //-------------------------------------------------------------------------
  }
}
