using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.DTO
{
    public class ParentMenuConfigDTO
    {
        public long ID { get; set; }
        public string MenuName { get; set; }
        public int WeixinID { get; set; } //属于哪个公众号
        public int MenuOrder { get; set; } //菜单排序
    }
}
