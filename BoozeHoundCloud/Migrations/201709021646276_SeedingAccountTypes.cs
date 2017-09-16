namespace BoozeHoundCloud.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class SeedingAccountTypes : DbMigration
  {
    public const string Bank = "Bank";
    public const string Income = "Income";
    public const string Expense = "Expense";
    public const string Creditor = "Creditor";
    public const string Debtor = "Debtor";

    public override void Up()
    {
      Sql($"INSERT INTO dbo.AccountTypes (Name) VALUES ('{Bank}')");
      Sql($"INSERT INTO dbo.AccountTypes (Name) VALUES ('{Income}')");
      Sql($"INSERT INTO dbo.AccountTypes (Name) VALUES ('{Expense}')");
      Sql($"INSERT INTO dbo.AccountTypes (Name) VALUES ('{Creditor}')");
      Sql($"INSERT INTO dbo.AccountTypes (Name) VALUES ('{Debtor}')");
    }

    public override void Down()
    {
      Sql("TRUNCATE TABLE dbo.AccountTypes");
    }
  }
}
