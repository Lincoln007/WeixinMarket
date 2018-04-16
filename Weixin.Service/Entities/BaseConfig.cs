using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Service.Entities
{
    public class BaseConfig:BaseEntity
    {
        private string weixinName;
        public string WeixinName
        {
            get { return weixinName; }
            set
            {
                if (value!=null)
                {
                    weixinName = value.Trim();
                }
            }
        }

        private string appid;
        public string Appid
        {
            get { return appid; }
            set
            {
                if (value!=null)
                {
                    appid = value.Trim();
                }
            }
        }

        private string token;
        public string Token
        {
            get { return token; }
            set
            {
                if (value!=null)
                {
                    token = value.Trim();
                }
            }
        }

        private string encodingAESKey;
        public string EncodingAESKey
        {
            get { return encodingAESKey; }
            set
            {
                if (value!=null)
                {
                    encodingAESKey = value.Trim();
                }
            }
        }

        private string appsecret;
        public string Appsecret
        {
            get { return appsecret; }
            set
            {
                if (value!=null)
                {
                    appsecret = value.Trim();
                }
            }
        }

        private string defaultResponse;
        public string DefaultResponse
        {
            get { return defaultResponse; }
            set
            {
                if (value!=null)
                {
                    defaultResponse = value.Trim();
                }
            }
        }

        public virtual User User { get; set; }
        public virtual ICollection<ParentMenuConfig> ParentMenuConfigs { get; set; } = new List<ParentMenuConfig>();
    }
}
