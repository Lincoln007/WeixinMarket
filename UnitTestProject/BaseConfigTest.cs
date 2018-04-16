using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.Service;
using Weixin.Service.Entities;

namespace UnitTestProject
{
    [TestClass]
    public class BaseConfigTest
    {
       public void AddNew()
        {
            using (var db = new WeixinDbContext())
            {
                var config = new BaseConfig()
                {
                    WeixinName = "test",
                    Appid = "123gwwg31g2g",
                    Token = "2dffw",
                    EncodingAESKey = "shf82hf28hfiwh82hf",
                    Appsecret = "29fh29hf2hf92hf92",
                    DefaultResponse = "for test"
                };
                db.BaseConfig.Add(config);
                db.SaveChanges();
            }
        }
    }
}
