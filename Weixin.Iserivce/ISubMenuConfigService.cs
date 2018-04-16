using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;

namespace Weixin.Iserivce
{
    public interface ISubMenuConfigService:IserviceSupport
    {
        Task<long> AddNew(string type, string menuName, string keyUrlMediaId, long parentMenuID, int menuOrder);
        Task Edit(long id, string type, string menuName, string keyUrlMediaId, long parentMenuID, int menuOrder);
        Task Delete(long id);
        Task<SubMenuConfigDTO[]> GetAll(long parentMenuID); //找到父菜单下的所有子菜单
    }
}
