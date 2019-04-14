namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Exception = c.String(maxLength: 2000),
                        ServiceName = c.String(maxLength: 50),
                        StatusId = c.Int(nullable: false),
                        ElapsedMilliseconds = c.Long(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AppSetting", "ParentId", c => c.Int());
            CreateIndex("dbo.AppSetting", "ParentId");
            AddForeignKey("dbo.AppSetting", "ParentId", "dbo.AppSetting", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppSetting", "ParentId", "dbo.AppSetting");
            DropIndex("dbo.AppSetting", new[] { "ParentId" });
            DropColumn("dbo.AppSetting", "ParentId");
            DropTable("dbo.ServiceLog");
        }
    }
}
