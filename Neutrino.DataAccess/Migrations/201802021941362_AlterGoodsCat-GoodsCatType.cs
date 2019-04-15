namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterGoodsCatGoodsCatType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoodsCategory", "GoodsRefId", c => c.Int(nullable: false));
            AddColumn("dbo.GoodsCategory", "GoodsCategoryTypeRefId", c => c.Int(nullable: false));
            AddColumn("dbo.GoodsCategoryType", "RefId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GoodsCategoryType", "RefId");
            DropColumn("dbo.GoodsCategory", "GoodsCategoryTypeRefId");
            DropColumn("dbo.GoodsCategory", "GoodsRefId");
        }
    }
}
