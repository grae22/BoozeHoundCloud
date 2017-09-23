using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.ViewModels
{
  public class TransactionFormViewModel
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    [Required]
    public decimal Value { get; set; }

    [Required]
    [DisplayName("Debit Account")]
    public int DebitAccountId { get; set; }

    public Account DebitAccount { get; set; }

    [Required]
    [DisplayName("Credit Account")]
    public int CreditAccountId { get; set; }

    public Account CreditAccount { get; set; }

    [MaxLength(Transaction.ReferenceMaxLength)]
    public string Reference { get; set; }

    [MaxLength(Transaction.DescriptionMaxLength)]
    public string Description { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public DateTime CreatedTimestamp { get; set; }

    public DateTime? ProcessedTimestamp { get; set; }

    public IList<Account> Accounts { get; set; }

    //-------------------------------------------------------------------------
  }
}