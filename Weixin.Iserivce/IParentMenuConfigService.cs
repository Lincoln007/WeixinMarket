using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;

namespace Weixin.Iserivce
{
    public interface IParentMenuConfigService:IserviceSupport
    {
        Task<long> AddNew(string menuName, int menuOrder, int weixinId);
        Task Edit(long id, string menuName, int menuOrder, int weixinId);
        Task Delete(long id);
        Task<ParentMenuConfigDTO[]> GetAll(int weixinId);
    }
}
