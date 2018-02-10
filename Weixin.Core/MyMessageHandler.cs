﻿using Senparc.Weixin.Context;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

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
            if (requestMessage.Content=="a")
            {
                CurrentMessageContext.StorageData = "aaa";
                responseMessage.Content = "你写入了缓存aaa";
            }
            else if (requestMessage.Content=="b")
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
            }
            return responseMessage;
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