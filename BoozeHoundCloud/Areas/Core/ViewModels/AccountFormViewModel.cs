using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.ViewModels
{
  public class AccountFormViewModel
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    public Guid UserId { get; set; }

    [Required]
    [MaxLength(Account.NameMaxLength)]
    public string Name { get; set; }

    [Required]
    [DisplayName("Account Type")]
    public int AccountTypeId { get; set; }

    public IEnumerable<AccountType> AccountTypes { get; set; }

    public decimal Balance { get; set; }

    //-------------------------------------------------------------------------
  }
}