using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Models.Core
{
  public interface IAccount
  {
    int Id { get; set; }
    string Name { get; set; }
    AccountType AccountType { get; set; }
    int AccountTypeId { get; set; }
    decimal Balance { get; set; }
  }

  public class Account : IAccount
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