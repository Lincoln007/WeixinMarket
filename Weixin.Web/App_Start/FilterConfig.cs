using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weixin.Core.Helper;

namespace Weixin.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterFilters(GlobalFilterCollection filters)
        {
            filters.Add(new JsonNetActionFilter());
            filters.Add(new MyAuthorizeFilter());
        }
    }
}