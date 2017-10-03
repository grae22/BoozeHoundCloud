using System;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  public interface IUserService
  {
    //-------------------------------------------------------------------------

    Guid CurrentUserId { get; }

    ApplicationUser GetUser(Guid id);

    //-------------------------------------------------------------------------
  }
}
