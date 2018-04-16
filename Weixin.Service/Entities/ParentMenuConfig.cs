using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service.Entities;

namespace Weixin.Service.Entities
{
    public class ParentMenuConfig:BaseEntity
    {
        private string menuName;//菜单名称
        public string MenuName
        {
            get { return menuName; }
            set
            {
                if (value!=null)
                {
                    menuName = value.Trim();
                }
            }
        }
        public int MenuOrder { get; set; } //菜单排序
        public virtual ICollection<SubMenuConfig> SubMenuConfigs { get; set; } = new List<SubMenuConfig>();

        public int WeixinID { get; set; } //属于哪个公众号
        public long BaseConfigId { get; set; }
        public virtual BaseConfig BaseConfig { get; set; }
    }
}
