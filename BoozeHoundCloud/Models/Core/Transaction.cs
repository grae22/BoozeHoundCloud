using System;
using System.ComponentModel.DataAnnotations;

namespace BoozeHoundCloud.Models.Core
{
  public class Transaction
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }

    public decimal Value { get; set; }

    public Account DebitAccount { get; set; }

    public int DebitAccountId { get; set; }

    public Account CreditAccount { get; set; }

    public int CreditAccountId { get; set; }

    [MaxLength(64)]
    public string Reference { get; set; }

    [MaxLength(256)]
    public string Description { get; set; }

    public DateTime Date { get; set; }

    public DateTime CreatedTimestamp { get; set; }

    public DateTime? ProcessedTimestamp { get; set; }

    //-------------------------------------------------------------------------
  }
}