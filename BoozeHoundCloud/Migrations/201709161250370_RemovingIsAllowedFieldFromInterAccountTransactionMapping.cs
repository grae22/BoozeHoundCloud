namespace BoozeHoundCloud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingIsAllowedFieldFromInterAccountTransactionMapping : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InterAccountTypeTransactionMappings", "IsAllowed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InterAccountTypeTransactionMappings", "IsAllowed", c => c.Boolean(nullable: false));
        }
    }
}
