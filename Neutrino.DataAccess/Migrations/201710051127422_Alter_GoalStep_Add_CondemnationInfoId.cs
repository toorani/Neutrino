namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_GoalStep_Add_CondemnationInfoId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GoalStep", name: "CondemnationInfo_Id", newName: "CondemnationInfoId");
            RenameIndex(table: "dbo.GoalStep", name: "IX_CondemnationInfo_Id", newName: "IX_CondemnationInfoId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.GoalStep", name: "IX_CondemnationInfoId", newName: "IX_CondemnationInfo_Id");
            RenameColumn(table: "dbo.GoalStep", name: "CondemnationInfoId", newName: "CondemnationInfo_Id");
        }
    }
}
