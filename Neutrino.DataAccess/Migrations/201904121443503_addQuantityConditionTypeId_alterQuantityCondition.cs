namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addQuantityConditionTypeId_alterQuantityCondition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuantityConditionType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Code = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.QuantityCondition", "QuantityConditionTypeId", c => c.Int());
            CreateIndex("dbo.QuantityCondition", "QuantityConditionTypeId");
            AddForeignKey("dbo.QuantityCondition", "QuantityConditionTypeId", "dbo.QuantityConditionType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuantityCondition", "QuantityConditionTypeId", "dbo.QuantityConditionType");
            DropIndex("dbo.QuantityCondition", new[] { "QuantityConditionTypeId" });
            DropColumn("dbo.QuantityCondition", "QuantityConditionTypeId");
            DropTable("dbo.QuantityConditionType");
        }
    }
}
