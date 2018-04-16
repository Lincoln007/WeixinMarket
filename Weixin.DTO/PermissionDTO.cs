using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.DTO
{
    public class PermissionDTO
    {
        public long Id { get; set; }

        [Display(Name ="权限描述")]
        public string Description { get; set; }

        [Display(Name = "权限名称")]
        public string PermissionName { get; set; }
    }
}
