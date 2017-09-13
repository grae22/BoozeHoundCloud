namespace BoozeHoundCloud.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class AddingInterAccountTypeTransactionMappings : DbMigration
  {
    public override void Up()
    {
      CreateTable(
          "dbo.InterAccountTypeTransactionMappings",
          c => new
          {
            Id = c.Int(nullable: false, identity: true),
            DebitAccountTypeId = c.Int(nullable: false),
            CreditAccountTypeId = c.Int(nullable: false),
            IsAllowed = c.Boolean(nullable: false),
          })
        .PrimaryKey(t => t.Id)
        .ForeignKey("dbo.AccountTypes", t => t.CreditAccountTypeId, cascadeDelete: false)
        .ForeignKey("dbo.AccountTypes", t => t.DebitAccountTypeId, cascadeDelete: false)
        .Index(t => t.DebitAccountTypeId)
        .Index(t => t.CreditAccountTypeId);
    }

    public override void Down()
    {
      DropForeignKey("dbo.InterAccountTypeTransactionMappings", "DebitAccountTypeId", "dbo.AccountTypes");
      DropForeignKey("dbo.InterAccountTypeTransactionMappings", "CreditAccountTypeId", "dbo.AccountTypes");
      DropIndex("dbo.InterAccountTypeTransactionMappings", new[] {"CreditAccountTypeId"});
      DropIndex("dbo.InterAccountTypeTransactionMappings", new[] {"DebitAccountTypeId"});
      DropTable("dbo.InterAccountTypeTransactionMappings");
    }
  }
}
