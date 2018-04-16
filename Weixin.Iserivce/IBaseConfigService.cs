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
        Task<long> AddNew(string weixinName, string appid, string token, string encodingAESKey, string appsecret,
            string defaultResponse);
        Task<BaseConfigDTO> GetById(long id);
        Task<BaseConfigDTO[]> GetAll();
        Task<BaseConfigDTO> GetByAppid(string appid);
        Task Edit(long id, string weixinName, string token, string encodingAESKey, string appsecret,
            string defaultResponse);
        Task Delete(long id);
        Task<BaseConfigDTO[]> GetByName(string weixinName);
    }
}
