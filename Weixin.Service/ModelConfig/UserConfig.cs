using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service.Entities;

namespace Weixin.Service.ModelConfig
{
    public class UserConfig: EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            ToTable("T_UserConfig");
            HasMany(u => u.Roles).WithMany(r => r.Users)
                .Map(m => m.ToTable("T_UserRole").MapLeftKey("UserId").MapRightKey("RoleId"));
            Property(u => u.UserName).HasMaxLength(50).IsRequired();
            Property(u => u.PhoneNum).HasMaxLength(50).IsRequired();
            Property(u => u.PasswordHash).HasMaxLength(100).IsRequired();
            Property(u => u.PasswordSalt).HasMaxLength(50).IsRequired();
        }
    }
}
