namespace BoozeHoundCloud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionDescriptionLengthReduced : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "Description", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transactions", "Description", c => c.String(maxLength: 256));
        }
    }
}
