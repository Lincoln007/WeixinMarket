using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service.Entities;

namespace Weixin.Service.ModelConfig
{
    public class RoleConfig: EntityTypeConfiguration<Role>
    {
        public RoleConfig()
        {
            ToTable("T_Roles");
            HasMany(r => r.Permissions).WithMany(p => p.Roles)
                .Map(m => m.ToTable("T_RolePermission").MapLeftKey("RoleId").MapRightKey("PermissionId"));
            Property(r => r.RoleName).HasMaxLength(50).IsRequired();
        }
    }
}
