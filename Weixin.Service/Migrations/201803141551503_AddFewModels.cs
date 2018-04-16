namespace Weixin.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFewModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_UserConfig",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        PhoneNum = c.String(nullable: false, maxLength: 50),
                        PasswordHash = c.String(nullable: false, maxLength: 100),
                        PasswordSalt = c.String(nullable: false, maxLength: 50),
                        IsDeleted = c.Boolean(nullable: false),
                        BaseConfig_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_BaseConfig", t => t.BaseConfig_Id)
                .ForeignKey("dbo.T_BaseConfig", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.BaseConfig_Id);
            
            CreateTable(
                "dbo.T_Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_Permissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        PermissionName = c.String(nullable: false, maxLength: 50),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_HandleConfigConfig",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AppId = c.String(nullable: false, maxLength: 50),
                        ClassName = c.String(nullable: false, maxLength: 50),
                        HandlerName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.T_RolePermission",
                c => new
                    {
                        RoleId = c.Long(nullable: false),
                        PermissionId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.PermissionId })
                .ForeignKey("dbo.T_Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.T_Permissions", t => t.PermissionId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.T_UserRole",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.T_UserConfig", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.T_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_UserConfig", "Id", "dbo.T_BaseConfig");
            DropForeignKey("dbo.T_UserRole", "RoleId", "dbo.T_Roles");
            DropForeignKey("dbo.T_UserRole", "UserId", "dbo.T_UserConfig");
            DropForeignKey("dbo.T_RolePermission", "PermissionId", "dbo.T_Permissions");
            DropForeignKey("dbo.T_RolePermission", "RoleId", "dbo.T_Roles");
            DropForeignKey("dbo.T_UserConfig", "BaseConfig_Id", "dbo.T_BaseConfig");
            DropIndex("dbo.T_UserRole", new[] { "RoleId" });
            DropIndex("dbo.T_UserRole", new[] { "UserId" });
            DropIndex("dbo.T_RolePermission", new[] { "PermissionId" });
            DropIndex("dbo.T_RolePermission", new[] { "RoleId" });
            DropIndex("dbo.T_UserConfig", new[] { "BaseConfig_Id" });
            DropIndex("dbo.T_UserConfig", new[] { "Id" });
            DropTable("dbo.T_UserRole");
            DropTable("dbo.T_RolePermission");
            DropTable("dbo.T_HandleConfigConfig");
            DropTable("dbo.T_Permissions");
            DropTable("dbo.T_Roles");
            DropTable("dbo.T_UserConfig");
        }
    }
}
