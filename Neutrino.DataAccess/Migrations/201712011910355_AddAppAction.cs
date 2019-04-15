namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppAction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationAction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        FaTitle = c.String(maxLength: 100),
                        ParentId = c.Int(),
                        Url = c.String(maxLength: 200),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationAction", t => t.ParentId)
                .Index(t => t.ParentId);
            
            AlterColumn("dbo.CostCoefficient", "Coefficient", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationAction", "ParentId", "dbo.ApplicationAction");
            DropIndex("dbo.ApplicationAction", new[] { "ParentId" });
            AlterColumn("dbo.CostCoefficient", "Coefficient", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.ApplicationAction");
        }
    }
}
