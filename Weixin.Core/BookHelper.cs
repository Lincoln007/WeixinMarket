using PlainElastic.Net;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Core
{
    public class BookHelper
    {
        public List<Book> Search(string keyword)
        {
            ElasticConnection client = new ElasticConnection("222.186.190.241", 9200);
            //第一个参数是数据库，第二个参数是表
            SearchCommand cmd = new SearchCommand("xiaoshuo", "xiaoshuo_url");
            var query = new QueryBuilder<Book>()
                .Query(b =>
                    b.Bool(m =>
                        //并且关系
                        m.Must(t =>
                            //分词的最小单位或关系查询
                            t.QueryString(t1 => t1.DefaultField("Name").Query(keyword))
                        )
                    )
                 ).Size(5).Build();
            var result = client.Post(cmd, query);
            var serializer = new JsonNetSerializer();
            var results = serializer.ToSearchResult<Book>(result); //把结果序列化
            List<Book> list = new List<Book>();
            Book book;
            foreach (var doc in results.Documents)
            {
                book = new Book()
                {
                    Name = doc.Name,
                    Url = doc.Url
                };
                list.Add(book);
            }
            return list;
        }

        public class Book
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
