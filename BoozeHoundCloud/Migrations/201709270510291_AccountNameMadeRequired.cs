namespace BoozeHoundCloud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountNameMadeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accounts", "Name", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accounts", "Name", c => c.String(maxLength: 64));
        }
    }
}
