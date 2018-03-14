using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Service.Entities
{
    /// <summary>
    /// 该类用来配置公众号对应的消息处理模式
    /// </summary>
    public class HandlerConfig:BaseEntity
    {
        private string appId; //公众号的appid
        public string AppId
        {
            get { return appId; }
            set
            {
                if (value!=null)
                {
                    appId = value.Trim();
                }
            }
        }

        private string className; //要加载的消息处理类的程序名称
        public string ClassName
        {
            get { return className; }
            set
            {
                if (value!=null)
                {
                    className = value.Trim();
                }
            }
        }

        private string handlerName;////要加载的消息处理类的书面名称
        public string HandlerName
        {
            get { return handlerName; }
            set
            {
                if (value!=null)
                {
                    handlerName = value.Trim();
                }
            }
        }
    }
}
