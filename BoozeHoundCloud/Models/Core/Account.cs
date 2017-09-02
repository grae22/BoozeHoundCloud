using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Models.Core
{
  public class Account
  {
    //-------------------------------------------------------------------------

    public const int NAME_MAX_LENGTH = 64;

    //-------------------------------------------------------------------------

    public int Id { get; set; }

    [MaxLength(NAME_MAX_LENGTH)]
    public string Name { get; set; }

    public AccountType AccountType { get; set; }

    public int AccountTypeId { get; set; }

    public decimal Balance { get; set; }

    //-------------------------------------------------------------------------
  }
}