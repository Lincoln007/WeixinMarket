using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weixin.Web.Models
{
    public class RoleEditModel
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string Referer { get; set; }
    }
}