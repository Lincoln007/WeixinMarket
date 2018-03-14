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
        [Required]
        public string WeixinName { get; set; }
        [Required]
        public string Appid { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string EncodingAESKey { get; set; }
        [Required]
        public string Appsecret { get; set; }

        [Display(Name = "默认回复消息")]
        public string DefaultResponse { get; set; }
    }
}
