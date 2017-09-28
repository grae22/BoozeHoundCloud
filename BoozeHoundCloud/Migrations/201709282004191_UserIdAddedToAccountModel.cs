namespace BoozeHoundCloud.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdAddedToAccountModel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Accounts", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Accounts", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Accounts", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Accounts", name: "UserId", newName: "User_Id");
        }
    }
}
