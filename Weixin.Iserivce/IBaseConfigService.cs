using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;

namespace Weixin.Iserivce
{
    public interface IBaseConfigService:IserviceSupport
    {
        long AddNew(string WeixinName, string Appid, string Token, string EncodingAESKey, string Appsecret);
        Task<BaseConfigDTO> GetById(long id);
        Task<BaseConfigDTO[]> GetAll();
        Task<int> GetByAppid(string appid);
        Task Create(string WeixinName, string Appid, string Token, string EncodingAESKey, string Appsecret,
            string DefaultResponse);
        Task Edit(long id, string WeixinName, string Token, string EncodingAESKey, string Appsecret,
            string DefaultResponse);
        Task Delete(long id);
    }
}
