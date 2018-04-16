using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service.Entities;

namespace Weixin.Service.ModelConfig
{
    public class SubMenuConfigConfig: EntityTypeConfiguration<SubMenuConfig>
    {
        public SubMenuConfigConfig()
        {
            ToTable("T_SubMenuConfig");
            Property(s => s.Type).HasMaxLength(50).IsRequired();
            Property(s => s.MenuName).HasMaxLength(50).IsRequired();
            Property(s => s.KeyUrlMediaId).IsRequired();
            Property(s => s.ParentMenuID).IsRequired();
            Property(s => s.MenuOrder).IsRequired();
            HasRequired(s => s.ParentMenuConfig).WithMany().HasForeignKey(e => e.ParentMenuID).WillCascadeOnDelete(false);
        }
    }
}
