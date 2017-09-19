using System;
using System.ComponentModel.DataAnnotations;
using BoozeHoundCloud.Areas.Core.Models;

namespace BoozeHoundCloud.Areas.Core.ViewModels
{
  public class TransactionFormViewModel
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    public decimal Value { get; set; }

    public int DebitAccountId { get; set; }

    public int CreditAccountId { get; set; }

    [MaxLength(Transaction.ReferenceMaxLength)]
    public string Reference { get; set; }

    [MaxLength(Transaction.DescriptionMaxLength)]
    public string Description { get; set; }

    public DateTime Date { get; set; }

    //-------------------------------------------------------------------------
  }
}