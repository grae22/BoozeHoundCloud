namespace BoozeHoundCloud.Areas.Core.Models
{
  public class InterAccountTypeTransactionMapping
  {
    //-------------------------------------------------------------------------

    public int Id { get; set; }
    public AccountType DebitAccountType { get; set; }
    public int DebitAccountTypeId { get; set; }
    public AccountType CreditAccountType { get; set; }
    public int CreditAccountTypeId { get; set; }

    //-------------------------------------------------------------------------
  }
}