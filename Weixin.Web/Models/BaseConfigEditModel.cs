using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Weixin.Web.Models
{
    public class BaseConfigEditModel
    {
        public long Id { get; set; }
        [Display(Name = "公众号名称")]
        public string WeixinName { get; set; } //公众号名称
        //public string Appid { get; set; } //这个不能修改
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
        public string Appsecret { get; set; }

        [Display(Name = "默认回复消息")]
        public string DefaultResponse { get; set; }
    }
}