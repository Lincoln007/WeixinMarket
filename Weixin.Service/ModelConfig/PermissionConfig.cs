using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service.Entities;

namespace Weixin.Service.ModelConfig
{
    public class PermissionConfig: EntityTypeConfiguration<Permission>
    {
        public PermissionConfig()
        {
            ToTable("T_Permissions");
            Property(p => p.PermissionName).HasMaxLength(50).IsRequired();
            Property(p => p.Description).IsOptional();
        }
    }
}
