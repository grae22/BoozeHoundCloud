using System.Data.Entity;
using NUnit.Framework;
using Moq;
using BoozeHoundCloud.DataAccess;
using BoozeHoundCloud.Models;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud_TestDataAccess
{
  [TestFixture]
  [Category("GenericRepository")]
  internal class GenericRepository_Test
  {
    //-------------------------------------------------------------------------

    private GenericRepository<Account> _testObject;
    private Mock<IDbSet<Account>> _dbSet;

    //-------------------------------------------------------------------------

    [SetUp]
    public void SetUp()
    {
      var context = new Mock<IApplicationDbContext>();

      _dbSet = new Mock<IDbSet<Account>>();
      _testObject = new GenericRepository<Account>(context.Object, _dbSet.Object);
    }

    //-------------------------------------------------------------------------
  }
}