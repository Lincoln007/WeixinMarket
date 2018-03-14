using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service.Entities;

namespace Weixin.Service.ModelConfig
{
    public class HandleConfigConfig : EntityTypeConfiguration<HandlerConfig>
    {
        public HandleConfigConfig()
        {
            ToTable("T_HandleConfigConfig");
            Property(h => h.AppId).HasMaxLength(50).IsRequired();
            Property(h => h.ClassName).HasMaxLength(50).IsRequired();
        }
    }
}
