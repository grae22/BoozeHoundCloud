using System;
using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Areas.Core.Models
{
  public class Transaction
  {
    //-------------------------------------------------------------------------

    public const int ReferenceMaxLength = 64;
    public const int DescriptionMaxLength = 128;

    //-------------------------------------------------------------------------

    public int Id { get; set; }

    public decimal Value { get; set; }

    // Virtual so entity framework can lazy load foreign key object.
    public virtual Account DebitAccount { get; set; }

    public int DebitAccountId { get; set; }

    // Virtual so entity framework can lazy load foreign key object.
    public virtual Account CreditAccount { get; set; }

    public int CreditAccountId { get; set; }

    [MaxLength(ReferenceMaxLength)]
    public string Reference { get; set; }

    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; }

    public DateTime Date { get; set; }

    public DateTime CreatedTimestamp { get; set; }

    public DateTime? ProcessedTimestamp { get; set; }

    //-------------------------------------------------------------------------
  }
}