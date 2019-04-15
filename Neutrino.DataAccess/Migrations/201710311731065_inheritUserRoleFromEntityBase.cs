namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inheritUserRoleFromEntityBase : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserRole", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.UserRole", name: "Role_Id", newName: "RoleId");
            RenameIndex(table: "dbo.UserRole", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.UserRole", name: "IX_Role_Id", newName: "IX_RoleId");
            AddColumn("dbo.Role", "DateCreated", c => c.DateTime());
            AddColumn("dbo.Role", "CreatorID", c => c.Int());
            AddColumn("dbo.Role", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "DateCreated", c => c.DateTime());
            AddColumn("dbo.User", "CreatorID", c => c.Int());
            AddColumn("dbo.User", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserClaim", "DateCreated", c => c.DateTime());
            AddColumn("dbo.UserClaim", "CreatorID", c => c.Int());
            AddColumn("dbo.UserClaim", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserClaim", "Deleted");
            DropColumn("dbo.UserClaim", "CreatorID");
            DropColumn("dbo.UserClaim", "DateCreated");
            DropColumn("dbo.User", "Deleted");
            DropColumn("dbo.User", "CreatorID");
            DropColumn("dbo.User", "DateCreated");
            DropColumn("dbo.Role", "Deleted");
            DropColumn("dbo.Role", "CreatorID");
            DropColumn("dbo.Role", "DateCreated");
            RenameIndex(table: "dbo.UserRole", name: "IX_RoleId", newName: "IX_Role_Id");
            RenameIndex(table: "dbo.UserRole", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.UserRole", name: "RoleId", newName: "Role_Id");
            RenameColumn(table: "dbo.UserRole", name: "UserId", newName: "User_Id");
        }
    }
}
