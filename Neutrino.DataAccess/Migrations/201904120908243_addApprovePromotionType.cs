namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addApprovePromotionType : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Goal", name: "CommissionId", newName: "PromotionId");
            RenameIndex(table: "dbo.Goal", name: "IX_CommissionId", newName: "IX_PromotionId");
            CreateTable(
                "dbo.ApprovePromotionType",
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
            
            AddColumn("dbo.Goal", "ApprovePromotionTypeId", c => c.Int());
            CreateIndex("dbo.Goal", "ApprovePromotionTypeId");
            AddForeignKey("dbo.Goal", "ApprovePromotionTypeId", "dbo.ApprovePromotionType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goal", "ApprovePromotionTypeId", "dbo.ApprovePromotionType");
            DropIndex("dbo.Goal", new[] { "ApprovePromotionTypeId" });
            DropColumn("dbo.Goal", "ApprovePromotionTypeId");
            DropTable("dbo.ApprovePromotionType");
            RenameIndex(table: "dbo.Goal", name: "IX_PromotionId", newName: "IX_CommissionId");
            RenameColumn(table: "dbo.Goal", name: "PromotionId", newName: "CommissionId");
        }
    }
}
