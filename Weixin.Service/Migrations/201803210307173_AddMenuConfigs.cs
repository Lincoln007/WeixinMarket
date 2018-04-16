namespace Weixin.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMenuConfigs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_ParentMenuConfig",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MenuName = c.String(nullable: false, maxLength: 50),
                    MenuOrder = c.Int(nullable: false),
                    WeixinID = c.Int(nullable: false),
                    BaseConfigId = c.Long(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_BaseConfig", t => t.BaseConfigId)
                .Index(t => t.BaseConfigId);

            CreateTable(
                "dbo.T_SubMenuConfig",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Type = c.String(nullable: false, maxLength: 50),
                    MenuName = c.String(nullable: false, maxLength: 50),
                    KeyUrlMediaId = c.String(nullable: false),
                    ParentMenuID = c.Long(nullable: false),
                    MenuOrder = c.Int(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.T_ParentMenuConfig", t => t.ParentMenuID)
                .Index(t => t.ParentMenuID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_SubMenuConfig", "ParentMenuID", "dbo.T_ParentMenuConfig");
            DropForeignKey("dbo.T_ParentMenuConfig", "BaseConfigId", "dbo.T_BaseConfig");
            DropIndex("dbo.T_SubMenuConfig", new[] { "ParentMenuID" });
            DropIndex("dbo.T_ParentMenuConfig", new[] { "BaseConfigId" });
            DropTable("dbo.T_SubMenuConfig");
            DropTable("dbo.T_ParentMenuConfig");
        }
    }
}
