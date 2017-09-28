namespace BoozeHoundCloud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingRequiredAttribFromAccountFKs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Accounts", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Accounts", new[] { "UserId" });
            AlterColumn("dbo.Accounts", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Accounts", "UserId");
            AddForeignKey("dbo.Accounts", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Accounts", new[] { "UserId" });
            AlterColumn("dbo.Accounts", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Accounts", "UserId");
            AddForeignKey("dbo.Accounts", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
