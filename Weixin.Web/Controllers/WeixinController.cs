using Senparc.Weixin;
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
            var messageHandler = new MyMessageHandler(Request.InputStream, postModel, config.DefaultResponse);//接收消息
            #region 记录日志#
            try
            {
                var logPath = Server.MapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}_{2}.txt", 
                    GetRandomFileName(),
                    messageHandler.RequestMessage.FromUserName,
                    messageHandler.RequestMessage.MsgType)));
                if (messageHandler.UsingEcryptMessage)
                {
                    messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}_{2}.txt",
                        GetRandomFileName(),
                        messageHandler.RequestMessage.FromUserName,
                        messageHandler.RequestMessage.MsgType)));
                }
                messageHandler.Execute();//执行微信处理过程
                return new FixWeixinBugWeixinResult(messageHandler);//返回结果
            }
            catch (Exception ex)
            {
                WeixinTrace.Log("messageHandler错误：" + ex.Message);
                using(TextWriter tw=new StreamWriter(Server.MapPath("~/App_Data/Error_" + GetRandomFileName() + ".txt")))
                {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);
                    //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);
                    if (messageHandler.ResponseDocument!=null)
                    {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }
                    if (ex.InnerException != null)
                    {
                        tw.WriteLine("========= InnerException =========");
                        tw.WriteLine(ex.InnerException.Message);
                        tw.WriteLine(ex.InnerException.Source);
                        tw.WriteLine(ex.InnerException.StackTrace);
                    }
                    tw.Flush();
                    tw.Close();
                }
            }
            #endregion#
            return Content(string.Empty);
        }
    }
}