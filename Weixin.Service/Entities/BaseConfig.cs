using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Service.Entities
{
    public class BaseConfig:BaseEntity
    {
        public string WeixinName { get; set; } //公众号名称
        public string Appid { get; set; }
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
        public string Appsecret { get; set; }
        public string DefaultResponse { get; set; } //默认回复消息
    }
}
