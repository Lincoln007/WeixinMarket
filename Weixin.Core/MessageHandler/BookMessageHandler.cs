using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using Senparc.Weixin.Entities.Request;
using System.Text;
using Weixin.Core.Helper;

namespace Weixin.Core
{
    public class BookMessageHandler : MessageHandler<MessageContext<IRequestMessageBase, IResponseMessageBase>>
    {
        PostModel postModel;
        string defaultResponse;
        public BookMessageHandler(Stream inputStream, PostModel postModel, string defaultResponse)
            : base(inputStream, postModel)
        {
            this.postModel = postModel;
            this.defaultResponse = defaultResponse;
            //Senparc.Weixin.Config.IsDebug = true;//开启日志记录状态
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
                .Default(() =>
                {
                    BookHelper bookHelper = new BookHelper();
                    var list = bookHelper.Search(requestMessage.Content.Replace("找", string.Empty));
                    StringBuilder sb = new StringBuilder();
                    foreach (var book in list)
                    {
                        sb.AppendLine(book.Name);
                        sb.AppendLine(book.Url);
                    }
                    responseMessage.Content = sb.ToString();
                    return responseMessage;
                });
            return handler.ResponseMessage as IResponseMessageBase;
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
