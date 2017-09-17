using System;
using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Areas.Core.Models
{
  public class AccountType : IEquatable<AccountType>
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    [MaxLength(16)]
    public string Name { get; set; }

    //-------------------------------------------------------------------------
    
    public bool Equals(AccountType other)
    {
      return Id == other?.Id;
    }

    //-------------------------------------------------------------------------
  }
}