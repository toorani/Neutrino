namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRefIdsToGoods : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Goods", "CompanyRefId", c => c.Int(nullable: false));
            AddColumn("dbo.Goods", "ProducerRefId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Goods", "ProducerRefId");
            DropColumn("dbo.Goods", "CompanyRefId");
        }
    }
}
