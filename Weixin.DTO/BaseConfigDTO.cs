using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.DTO
{
    public class BaseConfigDTO
    {
        public long Id { get; set; }
        [Display(Name = "公众号名称")]
        public string WeixinName { get; set; } //公众号名称
        public string Appid { get; set; }
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
        public string Appsecret { get; set; }

        [Display(Name = "默认回复消息")]
        public string DefaultResponse { get; set; }
    }
}
