using Senparc.Weixin;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Weixin.Core;
using Weixin.DTO;
using Weixin.Iserivce;
using Weixin.Service.Entities;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using System.Reflection;

namespace Weixin.Web.Controllers
{
    public class WeixinController : Controller
    {
        public IBaseConfigService BaseConfigService { get; set; }
        readonly Func<string> GetRandomFileName = () => DateTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);
        [HttpGet]
        [ActionName("Index")]
        public async Task<ContentResult> Get(PostModel postModel, string echostr)
        {
            long id = long.Parse(Request["id"]);
            var config = await BaseConfigService.GetById(id);
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, config.Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + config.Token + "," + config.Appid + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> Post(PostModel postModel)
        {
            long id = long.Parse(Request["id"]);
            var config = await BaseConfigService.GetById(id);
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, config.Token))
            {
                return Content("参数错误！");
            }
            postModel.Token = config.Token;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = config.EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = config.Appid;//wx5d8ab2a66119249e

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            //var messageHandler = new BookMessageHandler(Request.InputStream, postModel, config.DefaultResponse);//接收消息
            Assembly assembly = Assembly.Load("Weixin.Core");
            Type t = assembly.GetType("Weixin.Core.BookMessageHandler");
            Object[] constructParms = new object[] { Request.InputStream, postModel, config.DefaultResponse };
            var messageHandler = Activator.CreateInstance(t, constructParms)
                as MessageHandler<MessageContext<IRequestMessageBase, IResponseMessageBase>>;
            messageHandler.Execute();//执行微信处理过程
            return new FixWeixinBugWeixinResult(messageHandler);//返回结果
        }
    }
}