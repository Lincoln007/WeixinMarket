using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Service.Entities
{
    public class Permission:BaseEntity
    {
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value!=null)
                {
                    description = value.Trim();
                }
            }
        }

        private string permissionName;
        public string PermissionName
        {
            get { return permissionName; }
            set
            {
                if (value!=null)
                {
                    permissionName = value.Trim();
                }
            }
        }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
