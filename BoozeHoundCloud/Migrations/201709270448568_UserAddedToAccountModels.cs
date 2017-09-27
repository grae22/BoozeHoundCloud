namespace BoozeHoundCloud.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class UserAddedToAccountModels : DbMigration
  {
    public override void Up()
    {
      Sql("TRUNCATE TABLE dbo.Transactions");
      Sql("DELETE FROM dbo.Accounts");

      AddColumn("dbo.Accounts", "User_Id", c => c.String(maxLength: 128));
      CreateIndex("dbo.Accounts", "User_Id");
      AddForeignKey("dbo.Accounts", "User_Id", "dbo.AspNetUsers", "Id");
    }

    public override void Down()
    {
      DropForeignKey("dbo.Accounts", "User_Id", "dbo.AspNetUsers");
      DropIndex("dbo.Accounts", new[] {"User_Id"});
      DropColumn("dbo.Accounts", "User_Id");
    }
  }
}
