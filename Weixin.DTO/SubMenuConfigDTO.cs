using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.DTO
{
    public class SubMenuConfigDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string MenuName { get; set; }
        public string KeyUrlMediaId { get; set; }
        public long ParentMenuID { get; set; } //父菜单ID
        public string ParentMenuName { get; set; }
        public int MenuOrder { get; set; } //菜单排序
    }
}
