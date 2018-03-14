namespace Weixin.Service
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using Entities;

    public class WeixinDbContext : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“WeixinDbContext”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“Weixin.Service.WeixinDbContext”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“WeixinDbContext”
        //连接字符串。
        public WeixinDbContext()
            : base("name=WeixinDbContext")
        {
            Database.SetInitializer<WeixinDbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<BaseConfig> BaseConfig { get; set; }
        public DbSet<HandlerConfig> HandlerConfig { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
    }
}