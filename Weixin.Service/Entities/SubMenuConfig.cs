using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Service.Entities
{
    public class SubMenuConfig:BaseEntity
    {
        //菜单的响应动作类型，view表示网页类型，click表示点击类型，miniprogram表示小程序类型，parent是父菜单
        private string type;
        public string Type
        {
            get { return type; }
            set
            {
                if (value!=null)
                {
                    type = value.Trim();
                }
            }
        }

        private string menuName;
        public string MenuName
        {
            get { return menuName; }
            set
            {
                if (value != null)
                {
                    menuName = value.Trim();
                }
            }
        }

        //key为click等点击类型必须，url为view、miniprogram类型必须，mediaID为media_id类型和view_limited类型必须
        private string keyUrlMediaId;
        public string KeyUrlMediaId
        {
            get { return keyUrlMediaId; }
            set
            {
                if (value!=null)
                {
                    keyUrlMediaId = value.Trim();
                }
            }
        }
        public long ParentMenuID { get; set; } //父菜单ID

        public int MenuOrder { get; set; } //菜单排序

        public virtual ParentMenuConfig ParentMenuConfig { get; set; }
    }
}
