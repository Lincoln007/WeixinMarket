using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Weixin.Iserivce;
using Weixin.Service;

namespace Weixin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired(); //注册当前程序集的controllers
            var assemblies = new Assembly[] { Assembly.Load("Weixin.Service") }; //获取service程序集的类库
            builder.RegisterAssemblyTypes(assemblies) //注册刚刚获取的service程序集
                .Where(type => !type.IsAbstract //过滤要注册哪些程序集
                        && typeof(IserviceSupport).IsAssignableFrom(type))
                        .AsImplementedInterfaces().PropertiesAutowired();//只注册实现了IserviceSupport接口的
            var container = builder.Build();
            //注册系统级别的DependencyResolver，这样当MVC框架创建Controller等对象的时候都是管Autofac要对象。
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Senparc.Weixin.Threads.ThreadUtility.Register();
            var baseConfigService = new BaseConfigService();
            var task = baseConfigService.GetAll();
            var configs = task.Result;
            foreach (var config in configs)
            {
                Senparc.Weixin.MP.Containers.AccessTokenContainer.Register(config.Appid, config.Appsecret, config.WeixinName);
            }
        }
    }
}
