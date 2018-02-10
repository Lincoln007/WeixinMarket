namespace Weixin.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_BaseConfig",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WeixinName = c.String(nullable: false, maxLength: 50),
                        Appid = c.String(nullable: false, maxLength: 50),
                        Token = c.String(nullable: false, maxLength: 50),
                        EncodingAESKey = c.String(maxLength: 100),
                        Appsecret = c.String(nullable: false, maxLength: 50),
                        DefaultResponse = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.T_BaseConfig");
        }
    }
}
