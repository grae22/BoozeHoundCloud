using System;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public class UserService : IUserService
  {
    //-------------------------------------------------------------------------

    private readonly ApplicationDbContext _context;

    //-------------------------------------------------------------------------

    public UserService(IApplicationDbContext context)
    {
      if (context.GetType().IsAssignableFrom(typeof(ApplicationDbContext)) == false)
      {
        throw new ArgumentException("ApplicationDbContext is the only supported concrete type.");
      }

      _context = (ApplicationDbContext)context;
    }

    //-------------------------------------------------------------------------

    public ApplicationUser GetUser(Guid id)
    {
      return _context.Users.Find(id.ToString());
    }

    //-------------------------------------------------------------------------
  }
}