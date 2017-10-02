using System;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Services
{
  internal interface IUserService
  {
    //-------------------------------------------------------------------------

    ApplicationUser GetUser(Guid id);

    //-------------------------------------------------------------------------
  }
}
