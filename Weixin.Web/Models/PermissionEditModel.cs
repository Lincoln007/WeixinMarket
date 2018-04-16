using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weixin.Web.Models
{
    public class PermissionEditModel
    {
        public long PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Referer { get; set; }
    }
}