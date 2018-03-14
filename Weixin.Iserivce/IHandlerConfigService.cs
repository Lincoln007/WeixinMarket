using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;

namespace Weixin.Iserivce
{
    public interface IHandlerConfigService:IserviceSupport
    {
        Task<long> AddNew(string appId, string className, string handlerName);
        Task<HandlerConfigDTO> GetById(long id);
        Task<HandlerConfigDTO[]> GetAll();
        Task Edit(long id, string appId, string className, string handlerName);
        Task Delete(long id);
    }
}
