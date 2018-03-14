using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Service.Entities
{
    public class Role:BaseEntity
    {
        private string roleName;
        public string RoleName
        {
            get { return roleName; }
            set
            {
                if (value != null)
                {
                    roleName = value.Trim();
                }
            }
        }
        //既可以一张表对应一个Entity，关系表也建立实体，也可以像这样直接让对象带属性，隐式的关系表
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
