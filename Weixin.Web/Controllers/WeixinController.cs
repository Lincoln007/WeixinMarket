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
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Menu;

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

        //public async Task<ContentResult> CreateMenu()
        //{
        //    string id = string.IsNullOrEmpty(Request["id"]) ? "1" : Request["id"];
        //    var config = await BaseConfigService.GetById(long.Parse(id));
        //    var accessToken = await AccessTokenContainer.TryGetAccessTokenAsync(config.Appid,config.Appsecret);
        //    string msg = string.Empty;
        //    ButtonGroup bg = new ButtonGroup();
        //    List<SubMenuConfig> list = await BaseHandler.GetMenuConfig(id);
        //    if (list.Count > 0)
        //    {
        //        foreach (var item in list.Where(l => l.Class == 0)) //所有一级菜单
        //        {
        //            if (item.Type == "click") //添加所有单击菜单
        //            {
        //                bg.button.Add(new SingleClickButton()
        //                {
        //                    name = item.MenuName,
        //                    key = item.MenuKey
        //                });
        //            }
        //            else if (item.Type == "view") //添加所有视图菜单
        //            {
        //                bg.button.Add(new SingleViewButton()
        //                {
        //                    name = item.MenuName,
        //                    url = item.Url
        //                });
        //            }
        //            else if (item.Type == "parent") //含有子菜单的父级菜单
        //            {
        //                var subButton = new SubButton()
        //                {
        //                    name = item.MenuName
        //                };
        //                var subList = list.Where(l => l.ParentMenuID == item.MenuID); //找出该父菜单下的所有子菜单
        //                foreach (var subItem in subList)
        //                {
        //                    if (subItem.Type == "click") //添加所有单击菜单
        //                    {
        //                        subButton.sub_button.Add(new SingleClickButton()
        //                        {
        //                            name = subItem.MenuName,
        //                            key = subItem.MenuKey
        //                        });
        //                    }
        //                    else if (subItem.Type == "view") //添加所有视图菜单
        //                    {
        //                        subButton.sub_button.Add(new SingleViewButton()
        //                        {
        //                            name = subItem.MenuName,
        //                            url = subItem.Url
        //                        });
        //                    }
        //                }
        //                bg.button.Add(subButton);
        //            }
        //        }
        //    }
        //    var result = CommonApi.CreateMenu(accessToken, bg);
        //    return Content(result.errmsg);
        //}
    }
}