﻿using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Models.Core
{
  public class Account
  {
    //-------------------------------------------------------------------------

    public const int NameMaxLength = 64;

    //-------------------------------------------------------------------------

    public int Id { get; set; }

    [MaxLength(NameMaxLength)]
    public string Name { get; set; }

    // Virtual so entity framework can lazy load.
    public virtual AccountType AccountType { get; set; }

    public int AccountTypeId { get; set; }

    public decimal Balance { get; set; }

    //-------------------------------------------------------------------------
  }
}