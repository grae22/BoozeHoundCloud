using System.ComponentModel.DataAnnotations;
using BoozeHoundCloud.Models.Core;

namespace BoozeHoundCloud.Dtos
{
  public class AccountDto
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    [MaxLength(Account.NameMaxLength)]
    public string Name { get; set; }

    public int AccountTypeId { get; set; }

    public decimal Balance { get; set; }

    //-------------------------------------------------------------------------
  }
}