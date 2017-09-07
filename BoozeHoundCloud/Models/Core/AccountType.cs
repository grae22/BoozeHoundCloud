using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Models.Core
{
  public interface IAccountType
  {
    int Id { get; set; }
    string Name { get; set; }
  }

  public class AccountType : IAccountType
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    [MaxLength(16)]
    public string Name { get; set; }

    //-------------------------------------------------------------------------
  }
}