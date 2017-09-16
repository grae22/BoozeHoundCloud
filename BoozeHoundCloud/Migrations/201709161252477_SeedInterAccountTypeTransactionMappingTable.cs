namespace BoozeHoundCloud.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class SeedInterAccountTypeTransactionMappingTable : DbMigration
  {
    public override void Up()
    {
      AddPermissionToTransferFromAccountTypeAToB(SeedingAccountTypes.Bank, SeedingAccountTypes.Bank);
      AddPermissionToTransferFromAccountTypeAToB(SeedingAccountTypes.Bank, SeedingAccountTypes.Expense);
      AddPermissionToTransferFromAccountTypeAToB(SeedingAccountTypes.Bank, SeedingAccountTypes.Creditor);
      AddPermissionToTransferFromAccountTypeAToB(SeedingAccountTypes.Bank, SeedingAccountTypes.Debtor);
      AddPermissionToTransferFromAccountTypeAToB(SeedingAccountTypes.Income, SeedingAccountTypes.Bank);
      AddPermissionToTransferFromAccountTypeAToB(SeedingAccountTypes.Creditor, SeedingAccountTypes.Bank);
      AddPermissionToTransferFromAccountTypeAToB(SeedingAccountTypes.Debtor, SeedingAccountTypes.Bank);
    }

    public override void Down()
    {
      Sql("TRUNCATE TABLE dbo.InterAccountTypeTransactionMappings");
    }

    private void AddPermissionToTransferFromAccountTypeAToB(string fromAccountName, string toAccountName)
    {
      Sql(
        "DECLARE @debitAccountTypeId int " +
          "SET @debitAccountTypeId = (" +
          "SELECT Id " +
          "FROM AccountTypes " +
        $"WHERE Name = '{fromAccountName}') " +

        "DECLARE @creditAccountTypeId int " +
        "SET @creditAccountTypeId = (" +
          "SELECT Id " +
          "FROM AccountTypes " +
        $"WHERE Name = '{toAccountName}') " +

        "INSERT INTO dbo.InterAccountTypeTransactionMappings(DebitAccountTypeId, CreditAccountTypeId) " +
        "VALUES(@debitAccountTypeId, @creditAccountTypeId)"
      );
    }
  }
}
