using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Weixin.Service.Entities;

namespace Weixin.Service.ModelConfig
{
   public class BaseConfigConfig: EntityTypeConfiguration<BaseConfig>
    {
        public BaseConfigConfig()
        {
            ToTable("T_BaseConfig");
            HasRequired(b => b.User).WithRequiredDependent();
            Property(b => b.WeixinName).HasMaxLength(50).IsRequired();
            Property(b => b.Appid).HasMaxLength(50).IsRequired();
            Property(b => b.Token).HasMaxLength(50).IsRequired();
            Property(b => b.EncodingAESKey).HasMaxLength(100).IsOptional();
            Property(b => b.Appsecret).HasMaxLength(50).IsRequired();
            Property(b => b.DefaultResponse).HasColumnType("nvarchar(MAX)").IsOptional();
        }
    }
}
