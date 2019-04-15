namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredEndDate_goal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Goal", "ComputingTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Goal", "EndDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Goal", "ComputingTypeId");
            AddForeignKey("dbo.Goal", "ComputingTypeId", "dbo.ComputingType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goal", "ComputingTypeId", "dbo.ComputingType");
            DropIndex("dbo.Goal", new[] { "ComputingTypeId" });
            AlterColumn("dbo.Goal", "EndDate", c => c.DateTime());
            DropColumn("dbo.Goal", "ComputingTypeId");
        }
    }
}
