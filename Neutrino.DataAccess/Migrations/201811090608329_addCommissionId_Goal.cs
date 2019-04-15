namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommissionId_Goal : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Goal", name: "Commission_Id", newName: "CommissionId");
            RenameIndex(table: "dbo.Goal", name: "IX_Commission_Id", newName: "IX_CommissionId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Goal", name: "IX_CommissionId", newName: "IX_Commission_Id");
            RenameColumn(table: "dbo.Goal", name: "CommissionId", newName: "Commission_Id");
        }
    }
}
