namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_GoalStep : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RewardItemInfo", newName: "GoalStepItemInfo");
            DropIndex("dbo.GoalStep", new[] { "SellerRewardInfoId" });
            RenameColumn(table: "dbo.GoalStep", name: "InvoiceRewardInfoId", newName: "CondemnationInfo_Id");
            RenameColumn(table: "dbo.GoalStep", name: "SellerRewardInfoId", newName: "RewardInfoId");
            RenameIndex(table: "dbo.GoalStep", name: "IX_InvoiceRewardInfoId", newName: "IX_CondemnationInfo_Id");
            AddColumn("dbo.ComputingType", "Code", c => c.Int());
            AddColumn("dbo.CondemnationType", "Code", c => c.Int());
            AddColumn("dbo.GoalStep", "ComputingValue", c => c.Single(nullable: false));
            AddColumn("dbo.GoalStepItemInfo", "EachValue", c => c.Int(nullable: false));
            AddColumn("dbo.OtherRewardType", "Code", c => c.Int());
            AddColumn("dbo.RewardType", "Code", c => c.Int());
            AddColumn("dbo.GoodsCategoryType", "Code", c => c.Int());
            AlterColumn("dbo.GoalStep", "RewardInfoId", c => c.Int(nullable: false));
            AlterColumn("dbo.GoalStepItemInfo", "Amount", c => c.Single(nullable: false));
            CreateIndex("dbo.GoalStep", "RewardInfoId");
            DropColumn("dbo.GoalStepItemInfo", "ItemValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoalStepItemInfo", "ItemValue", c => c.Int(nullable: false));
            DropIndex("dbo.GoalStep", new[] { "RewardInfoId" });
            AlterColumn("dbo.GoalStepItemInfo", "Amount", c => c.Int(nullable: false));
            AlterColumn("dbo.GoalStep", "RewardInfoId", c => c.Int());
            DropColumn("dbo.GoodsCategoryType", "Code");
            DropColumn("dbo.RewardType", "Code");
            DropColumn("dbo.OtherRewardType", "Code");
            DropColumn("dbo.GoalStepItemInfo", "EachValue");
            DropColumn("dbo.GoalStep", "ComputingValue");
            DropColumn("dbo.CondemnationType", "Code");
            DropColumn("dbo.ComputingType", "Code");
            RenameIndex(table: "dbo.GoalStep", name: "IX_CondemnationInfo_Id", newName: "IX_InvoiceRewardInfoId");
            RenameColumn(table: "dbo.GoalStep", name: "RewardInfoId", newName: "SellerRewardInfoId");
            RenameColumn(table: "dbo.GoalStep", name: "CondemnationInfo_Id", newName: "InvoiceRewardInfoId");
            CreateIndex("dbo.GoalStep", "SellerRewardInfoId");
            RenameTable(name: "dbo.GoalStepItemInfo", newName: "RewardItemInfo");
        }
    }
}
