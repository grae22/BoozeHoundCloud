using System;
using System.ComponentModel.DataAnnotations;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.DataTransferObjects
{
  public class AccountDto
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    public Guid UserId { get; set; }

    [MaxLength(Account.NameMaxLength)]
    public string Name { get; set; }

    public int AccountTypeId { get; set; }

    public decimal Balance { get; set; }

    //-------------------------------------------------------------------------
  }
}