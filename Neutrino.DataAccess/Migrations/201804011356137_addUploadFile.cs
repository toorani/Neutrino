namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUploadFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UploadedFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginalFileName = c.String(),
                        UploadedFileName = c.String(),
                        HashValue = c.String(),
                        CompanyId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            AlterColumn("dbo.SalesLedger", "StartDate", c => c.DateTime());
            AlterColumn("dbo.StockInventory", "StartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UploadedFile", "CompanyId", "dbo.Company");
            DropIndex("dbo.UploadedFile", new[] { "CompanyId" });
            AlterColumn("dbo.StockInventory", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SalesLedger", "StartDate", c => c.DateTime(nullable: false));
            DropTable("dbo.UploadedFile");
        }
    }
}
