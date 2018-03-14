using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Service.Entities
{
    public class User:BaseEntity
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                if (value!=null)
                {
                    userName = value.Trim();
                }
            }
        }

        private string phoneNum;
        public string PhoneNum
        {
            get { return phoneNum; }
            set
            {
                if (value!=null)
                {
                    phoneNum = value.Trim();
                }
            }
        }

        private string passwordHash;
        public string PasswordHash { get; set; }

        private string passwordSalt;
        public string PasswordSalt
        {
            get { return passwordSalt; }
            set
            {
                if (value!=null)
                {
                    passwordSalt = value.Trim();
                }
            }
        }

        public BaseConfig BaseConfig { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
