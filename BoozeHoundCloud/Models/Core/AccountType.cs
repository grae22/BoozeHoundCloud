using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Models.Core
{
  public class AccountType : IAccountType
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    [MaxLength(16)]
    public string Name { get; set; }

    //-------------------------------------------------------------------------
  }
}