namespace BoozeHoundCloud.Models.Core
{
  public interface IAccount
  {
    int Id { get; set; }
    string Name { get; set; }
    AccountType AccountType { get; set; }
    int AccountTypeId { get; set; }
    decimal Balance { get; set; }
  }
}