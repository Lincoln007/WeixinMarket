﻿using Autofac;
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
using Weixin.Core.Helper;
using Weixin.Web.App_Start;

namespace Weixin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired(); //注册当前程序集的controllers

            var service_assemblies = new Assembly[] { Assembly.Load("Weixin.Service") }; //获取service程序集的类库
            builder.RegisterAssemblyTypes(service_assemblies) //注册刚刚获取的service程序集
                .Where(type => !type.IsAbstract //过滤要注册哪些程序集
                        && typeof(IserviceSupport).IsAssignableFrom(type))
                        .AsImplementedInterfaces().PropertiesAutowired();//只注册实现了IserviceSupport接口的

            var container = builder.Build();
            //注册系统级别的DependencyResolver，这样当MVC框架创建Controller等对象的时候都是管Autofac要对象。
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Senparc.Weixin.Threads.ThreadUtility.Register();

            //MvcApplication不是autofac创建，所以不会自动属性注入
            var baseConfigService = (IBaseConfigService)DependencyResolver.Current.GetService(typeof(IBaseConfigService));
            var configs = baseConfigService.GetAll().Result;
            foreach (var config in configs) //应用程序初始化时注册所有accesstoken
            {
                Senparc.Weixin.MP.Containers.AccessTokenContainer.Register(config.Appid, config.Appsecret, config.WeixinName);
            }
            Senparc.Weixin.Config.IsDebug = true;//开启日志记录状态
            FilterConfig.RegisterFilters(GlobalFilters.Filters);
        }
    }
}
