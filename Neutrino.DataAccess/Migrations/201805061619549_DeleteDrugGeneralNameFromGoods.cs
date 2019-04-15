namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDrugGeneralNameFromGoods : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Goods", "DrugGeneralName_Id", "dbo.DrugGeneralName");
            DropIndex("dbo.Goods", new[] { "DrugGeneralName_Id" });
            AddColumn("dbo.DrugGeneralName", "GoodsId", c => c.Int(nullable: false));
            CreateIndex("dbo.DrugGeneralName", "GoodsId");
            AddForeignKey("dbo.DrugGeneralName", "GoodsId", "dbo.Goods", "Id");
            DropColumn("dbo.Goods", "DrugGeneralName_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Goods", "DrugGeneralName_Id", c => c.Int());
            DropForeignKey("dbo.DrugGeneralName", "GoodsId", "dbo.Goods");
            DropIndex("dbo.DrugGeneralName", new[] { "GoodsId" });
            DropColumn("dbo.DrugGeneralName", "GoodsId");
            CreateIndex("dbo.Goods", "DrugGeneralName_Id");
            AddForeignKey("dbo.Goods", "DrugGeneralName_Id", "dbo.DrugGeneralName", "Id");
        }
    }
}
