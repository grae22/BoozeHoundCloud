namespace BoozeHoundCloud.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class RequiredFieldsSetForAccounts : DbMigration
  {
    public override void Up()
    {
      Sql("DELETE FROM dbo.Accounts");

      DropForeignKey("dbo.Accounts", "User_Id", "dbo.AspNetUsers");
      DropIndex("dbo.Accounts", new[] {"User_Id"});
      AlterColumn("dbo.Accounts", "User_Id", c => c.String(nullable: false, maxLength: 128));
      CreateIndex("dbo.Accounts", "User_Id");
      AddForeignKey("dbo.Accounts", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
    }

    public override void Down()
    {
      DropForeignKey("dbo.Accounts", "User_Id", "dbo.AspNetUsers");
      DropIndex("dbo.Accounts", new[] {"User_Id"});
      AlterColumn("dbo.Accounts", "User_Id", c => c.String(maxLength: 128));
      CreateIndex("dbo.Accounts", "User_Id");
      AddForeignKey("dbo.Accounts", "User_Id", "dbo.AspNetUsers", "Id");
    }
  }
}
