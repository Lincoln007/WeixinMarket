using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weixin.Core.Helper;
using Weixin.Iserivce;

namespace Weixin.Web.App_Start
{
    public class MyAuthorizeFilter : IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //获得当前要执行的Action上标注的CheckPermissionAttribute实例对象
            CheckPermissionAttribute[] permissionAttributes = (CheckPermissionAttribute[])filterContext
                .ActionDescriptor.GetCustomAttributes(typeof(CheckPermissionAttribute), false);
            if (permissionAttributes==null)
            {
                return; //没有标注任何CheckPermissionAttribute，则不用检查
            }

            long? userId = (long?)filterContext.HttpContext.Session["userid"];
            if (userId==null) //没有userid，说明没有登录
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    AjaxResult ajaxResult = new AjaxResult()
                    {
                        Status = "redirect",
                        Data = "/Main/Login",
                        ErrorMsg = "没有登录"
                    };
                }
            }
            //由于ZSZAuthorizeFilter不是被autofac创建，因此不会自动进行属性的注入
            //需要手动获取Service对象
            IUserService userService = (IUserService)DependencyResolver.Current.GetService(typeof(IUserService));
            foreach (var item in permissionAttributes) //遍历控制器标注的所有权限
            {
                if (!userService.HasPermission(userId.Value,item.Permission)) //检查是否有任一权限
                {
                    //在IAuthorizationFilter里面，只要修改filterContext.Result 
                    //那么真正的Action方法就不会执行了
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        AjaxResult ajaxResult = new AjaxResult();
                        ajaxResult.Status = "error";
                        ajaxResult.ErrorMsg = "没有[" + item.Permission + "]权限";
                        filterContext.Result = new JsonNetResult { Data = ajaxResult };
                    }
                    else
                    {
                        filterContext.Result = new ContentResult { Content = "没有[" + item.Permission + "]权限" };
                    }
                    return;
                }
            }
        }
    }
}