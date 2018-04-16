using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.DTO
{
    public class RoleDTO
    {
        public long Id { get; set; }

        [Display(Name = "角色名称")]
        public string RoleName { get; set; }
    }
}
