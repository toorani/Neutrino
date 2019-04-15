namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterOrgStruct : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PersonelShare", newName: "OrgStructureShare");
            AddColumn("dbo.OrgStructure", "PositionTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrgStructure", "PositionTypeId");
            AddForeignKey("dbo.OrgStructure", "PositionTypeId", "dbo.PositionType", "Id");
            DropColumn("dbo.OrgStructure", "Title");
            DropColumn("dbo.OrgStructure", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrgStructure", "Code", c => c.String());
            AddColumn("dbo.OrgStructure", "Title", c => c.String());
            DropForeignKey("dbo.OrgStructure", "PositionTypeId", "dbo.PositionType");
            DropIndex("dbo.OrgStructure", new[] { "PositionTypeId" });
            DropColumn("dbo.OrgStructure", "PositionTypeId");
            RenameTable(name: "dbo.OrgStructureShare", newName: "PersonelShare");
        }
    }
}
