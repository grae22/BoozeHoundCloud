using System.ComponentModel.DataAnnotations;
using BoozeHoundCloud.Models;

namespace BoozeHoundCloud.Areas.Core.Models
{
  public class Account
  {
    //-------------------------------------------------------------------------

    public const int NameMaxLength = 64;

    //-------------------------------------------------------------------------

    public int Id { get; set; }

    public ApplicationUser User { get; set; }

    [MaxLength(NameMaxLength)]
    public string Name { get; set; }

    // Virtual so entity framework can lazy load foreign key object.
    public virtual AccountType AccountType { get; set; }

    public int AccountTypeId { get; set; }

    public decimal Balance { get; set; }

    //-------------------------------------------------------------------------
  }
}