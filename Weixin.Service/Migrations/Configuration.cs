namespace Weixin.Service.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<Weixin.Service.WeixinDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Weixin.Service.WeixinDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.BaseConfig.AddOrUpdate(
              new BaseConfig
              {
                  WeixinName="²âÊÔºÅ",
                  Appid= "wx4fcabfbbd43435e5",
                  Token="liqitest",
                  Appsecret= "c2d76ba2c7bc896d79d27bb8b12ded38",
                  EncodingAESKey="",
                  DefaultResponse="»¶Ó­¹Ø×¢"
              }

            );

        }
    }
}
