namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_useraccountentities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserLogin", "UserId", "dbo.User");
            DropIndex("dbo.UserLogin", new[] { "UserId" });
            AddColumn("dbo.User", "PhoneNumber", c => c.String());
            AddColumn("dbo.User", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Role", "IsUsingBySystem", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserRole", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.User", "Email", c => c.String());
            AlterColumn("dbo.User", "UserName", c => c.String());
            AlterColumn("dbo.User", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.User", "Name", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.User", "LastName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.UserClaim", "DateCreated", c => c.DateTime());
            DropColumn("dbo.User", "MobileNumber");
            DropColumn("dbo.User", "MobileNumberConfirmed");
            DropTable("dbo.UserLogin");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId });
            
            AddColumn("dbo.User", "MobileNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "MobileNumber", c => c.String());
            AlterColumn("dbo.UserClaim", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.User", "LastName", c => c.String(maxLength: 256));
            AlterColumn("dbo.User", "Name", c => c.String(maxLength: 256));
            AlterColumn("dbo.User", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.User", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.UserRole", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Role", "IsUsingBySystem", c => c.Boolean());
            DropColumn("dbo.User", "PhoneNumberConfirmed");
            DropColumn("dbo.User", "PhoneNumber");
            CreateIndex("dbo.UserLogin", "UserId");
            AddForeignKey("dbo.UserLogin", "UserId", "dbo.User", "Id");
        }
    }
}
