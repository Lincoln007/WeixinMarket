using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Core;
using Weixin.Core.CommonHelper;
using static Weixin.Core.CommonHelper.BookHelper;
using System.IO;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using System.Reflection;

namespace UnitTestProject
{
    [TestClass]
    public class BookHelperTest
    {
        public void Search()
        {
            BookHelper bookHelper = new BookHelper();
            ElasticConnection client = new ElasticConnection("222.186.190.241", 9200);
            //第一个参数是数据库，第二个参数是表
            SearchCommand cmd = new SearchCommand("xiaoshuo", "xiaoshuo_url");
            var query = new QueryBuilder<Book>()
                .Query(b =>
                    b.Bool(m =>
                        //并且关系
                        m.Must(t =>
                            //分词的最小单位或关系查询
                            t.QueryString(t1 => t1.DefaultField("Name").Query("总裁"))
                        )
                    )
                 ).Size(5).Build();
            var result = client.Post(cmd, query);
            var count = result.Result.Count();
            var serializer = new JsonNetSerializer();
            var results = serializer.ToSearchResult<Book>(result); //把结果序列化
            Book book;
            foreach (var doc in results.Documents)
            {
                book = new Book()
                {
                    Name = doc.Name,
                    Url = doc.Url
                };
                Console.WriteLine(book.Name);
                Console.WriteLine(book.Url);
            }
        }

        [TestMethod]
        public void Reflection()
        {
            Assembly assembly = Assembly.LoadFrom("Weixin.Core.dll");
            Type t = assembly.GetType("Weixin.Core.BookMessageHandler");
            Object[] constructParms = new object[] { new MemoryStream(), new PostModel(), string.Empty };
            var messageHandler = Activator.CreateInstance(t, constructParms)
                as MessageHandler<MessageContext<IRequestMessageBase, IResponseMessageBase>>;
            messageHandler.Execute();//执行微信处理过程
        }
    }

    
}
