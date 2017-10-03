using System;
using System.Web;
using BoozeHoundCloud.Models;
using Microsoft.AspNet.Identity;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public class UserService : IUserService
  {
    //-------------------------------------------------------------------------

    public Guid CurrentUserId => new Guid(HttpContext.Current.User.Identity.GetUserId());

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