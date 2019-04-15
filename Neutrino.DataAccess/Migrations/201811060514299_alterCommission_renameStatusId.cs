namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterCommission_renameStatusId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Commission", name: "CommissionStatusId", newName: "StatusId");
            RenameIndex(table: "dbo.Commission", name: "IX_CommissionStatusId", newName: "IX_StatusId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Commission", name: "IX_StatusId", newName: "IX_CommissionStatusId");
            RenameColumn(table: "dbo.Commission", name: "StatusId", newName: "CommissionStatusId");
        }
    }
}
