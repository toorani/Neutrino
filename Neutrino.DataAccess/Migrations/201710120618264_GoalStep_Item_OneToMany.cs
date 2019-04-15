namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoalStep_Item_OneToMany : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.GoalStep DROP CONSTRAINT [FK_dbo.GoalStep_dbo.RewardItemInfo_SellerRewardInfoId]");
            Sql("ALTER TABLE dbo.GoalStep DROP CONSTRAINT [FK_dbo.GoalStep_dbo.RewardItemInfo_InvoiceRewardInfoId]");
            DropForeignKey("dbo.GoalStep", "CondemnationInfoId", "dbo.GoalStepItemInfo");
            DropForeignKey("dbo.GoalStep", "CondemnationTypeId", "dbo.CondemnationType");
            DropForeignKey("dbo.GoalStep", "OtherRewardId", "dbo.OtherRewardType");
            DropForeignKey("dbo.GoalStep", "RewardInfoId", "dbo.GoalStepItemInfo");
            DropForeignKey("dbo.GoalStep", "RewardTypeId", "dbo.RewardType");

            DropIndex("dbo.GoalStep", new[] { "RewardTypeId" });
            DropIndex("dbo.GoalStep", new[] { "RewardInfoId" });
            DropIndex("dbo.GoalStep", new[] { "CondemnationTypeId" });
            DropIndex("dbo.GoalStep", new[] { "CondemnationInfoId" });
            DropIndex("dbo.GoalStep", new[] { "OtherRewardId" });
            CreateTable(
                "dbo.GoalStepActionType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Code = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.GoalStepItemInfo", "ActionTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.GoalStepItemInfo", "ItemTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.GoalStepItemInfo", "ChoiceValueId", c => c.Int());
            AlterColumn("dbo.GoalStepItemInfo", "Amount", c => c.Single());
            CreateIndex("dbo.GoalStepItemInfo", "ActionTypeId");
            CreateIndex("dbo.GoalStepItemInfo", "GoalStepId");
            AddForeignKey("dbo.GoalStepItemInfo", "ActionTypeId", "dbo.GoalStepActionType", "Id");
            AddForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep", "Id");
            
            DropColumn("dbo.GoalStep", "RewardTypeId");
            DropColumn("dbo.GoalStep", "RewardInfoId");
            DropColumn("dbo.GoalStep", "CondemnationTypeId");
            DropColumn("dbo.GoalStep", "CondemnationInfoId");
            DropColumn("dbo.GoalStep", "OtherRewardId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoalStep", "OtherRewardId", c => c.Int());
            AddColumn("dbo.GoalStep", "CondemnationInfoId", c => c.Int());
            AddColumn("dbo.GoalStep", "CondemnationTypeId", c => c.Int());
            AddColumn("dbo.GoalStep", "RewardInfoId", c => c.Int());
            AddColumn("dbo.GoalStep", "RewardTypeId", c => c.Int());
            DropForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep");
            DropForeignKey("dbo.GoalStepItemInfo", "ActionTypeId", "dbo.GoalStepActionType");
            DropIndex("dbo.GoalStepItemInfo", new[] { "GoalStepId" });
            DropIndex("dbo.GoalStepItemInfo", new[] { "ActionTypeId" });
            AlterColumn("dbo.GoalStepItemInfo", "Amount", c => c.Single(nullable: false));
            DropColumn("dbo.GoalStepItemInfo", "ChoiceValueId");
            DropColumn("dbo.GoalStepItemInfo", "ItemTypeId");
            DropColumn("dbo.GoalStepItemInfo", "ActionTypeId");
            DropTable("dbo.GoalStepActionType");
            CreateIndex("dbo.GoalStep", "OtherRewardId");
            CreateIndex("dbo.GoalStep", "CondemnationInfoId");
            CreateIndex("dbo.GoalStep", "CondemnationTypeId");
            CreateIndex("dbo.GoalStep", "RewardInfoId");
            CreateIndex("dbo.GoalStep", "RewardTypeId");
            AddForeignKey("dbo.GoalStep", "RewardTypeId", "dbo.RewardType", "Id");
            AddForeignKey("dbo.GoalStep", "RewardInfoId", "dbo.GoalStepItemInfo", "Id");
            AddForeignKey("dbo.GoalStep", "OtherRewardId", "dbo.OtherRewardType", "Id");
            AddForeignKey("dbo.GoalStep", "CondemnationTypeId", "dbo.CondemnationType", "Id");
            AddForeignKey("dbo.GoalStep", "CondemnationInfoId", "dbo.GoalStepItemInfo", "Id");
        }
    }
}
