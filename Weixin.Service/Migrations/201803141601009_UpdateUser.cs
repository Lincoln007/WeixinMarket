namespace Weixin.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.T_UserConfig", newName: "T_User");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.T_User", newName: "T_UserConfig");
        }
    }
}
