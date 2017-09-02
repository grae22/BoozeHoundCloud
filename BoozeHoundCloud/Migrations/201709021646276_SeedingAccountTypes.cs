namespace BoozeHoundCloud.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class SeedingAccountTypes : DbMigration
  {
    public override void Up()
    {
      Sql("INSERT INTO AccountTypes (Name) VALUES ('Bank')");
      Sql("INSERT INTO AccountTypes (Name) VALUES ('Income')");
      Sql("INSERT INTO AccountTypes (Name) VALUES ('Expense')");
      Sql("INSERT INTO AccountTypes (Name) VALUES ('Creditor')");
      Sql("INSERT INTO AccountTypes (Name) VALUES ('Debtor')");
    }

    public override void Down()
    {
    }
  }
}
