using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Senparc.Weixin.Entities.Request;

namespace Weixin.Core
{
    public class MyMessageHandler: MessageHandler<MessageContext<IRequestMessageBase, IResponseMessageBase>>
    {
        PostModel postModel;
        string defaultResponse;
        public MyMessageHandler(Stream inputStream, PostModel postModel, string defaultResponse)
            : base(inputStream, postModel)
        {
            this.postModel = postModel;
            this.defaultResponse = defaultResponse;
        }
        //public MyMessageHandler(Stream inputStream, PostModel postModel=null,int maxRecordCount=0,
        //    DeveloperInfo developerInfo = null) : base(inputStream, postModel, maxRecordCount, developerInfo)
        //{
        //    base.CurrentMessageContext.ExpireMinutes = 10;
        //}
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = defaultResponse;
            return responseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = defaultResponse;
            var handler = requestMessage.StartHandler(false)
                .Keyword("a", () =>
                {
                    CurrentMessageContext.StorageData = "aaaa";
                    responseMessage.Content = "你写入了缓存aaaa";
                    return responseMessage;
                })
                .Keyword("b", () =>
                 {
                     var temp = CurrentMessageContext.StorageData;
                     if (temp != null)
                     {
                         responseMessage.Content = temp.ToString();
                     }
                     else
                     {
                         responseMessage.Content = "没有缓存";
                     }
                     return responseMessage;
                 })
                 .Keyword("c", () =>
                 {
                     var responseMessageNews = base.CreateResponseMessage<ResponseMessageNews>();
                     var news = new Article()
                     {
                         Title = "一路繁花相送",
                         Description = "一路繁花相送简介",
                         PicUrl = "https://wx2.sinaimg.cn/mw690/ab72f980gy1fobpytwswvj20d60kytj8.jpg",
                         Url = "https://m.weibo.cn/u/2876438912?jumpfrom=weibocom"
                     };
                     responseMessageNews.Articles.Add(news);
                     return responseMessageNews;
                 });
            return handler.ResponseMessage as IResponseMessageBase;
            //if (requestMessage.Content=="a")
            //{
                
            //}
            //else if (requestMessage.Content=="b")
            //{
               
            //}
            //return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            switch (requestMessage.EventKey)
            {
                case "cooperation_13":
                    responseMessage.Content = "请发邮件到hiliqi@gmail.com"; break;
                default:
                    responseMessage.Content = "没有接收到消息";
                    break;
            }
            return responseMessage;
        }
    }
}
