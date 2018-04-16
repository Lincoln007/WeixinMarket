using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service.Entities;

namespace Weixin.Service.ModelConfig
{
    public class ParentMenuConfigConfig: EntityTypeConfiguration<ParentMenuConfig>
    {
        public ParentMenuConfigConfig()
        {
            ToTable("T_ParentMenuConfig");
            HasRequired(p => p.BaseConfig).WithMany().HasForeignKey(p => p.BaseConfigId).WillCascadeOnDelete(false);
            Property(p => p.MenuName).HasMaxLength(50).IsRequired();
            Property(p => p.WeixinID).IsRequired();
            Property(p => p.MenuOrder).IsRequired();
        }
    }
}
