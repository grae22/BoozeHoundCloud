using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Models.Core
{
  public class Account
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    [MaxLength(64)]
    public string Name { get; set; }

    public AccountType AccountType { get; set; }

    public int AccountTypeId { get; set; }

    public decimal Balance { get; set; }

    //-------------------------------------------------------------------------
  }
}