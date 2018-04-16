using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;

namespace Weixin.Iserivce
{
    public interface IPermissionService:IserviceSupport
    {
        Task<long> AddNew(string permissionName, string description);
        Task Edit(long id, string permissionName, string description);
        Task Delete(long id);
        Task<PermissionDTO> GetById(long id);
        Task<PermissionDTO[]> GetAll();
        Task<PermissionDTO[]> GetByName(string permissionName);//GetByName("User.Add")
        Task<PermissionDTO[]> GetByRoleId(long roleId);
        Task AddPermissonIds(long roleId, long[] permissionIds);
        Task EditPermissonIds(long roleId, long[] permissionIds);
    }
}
