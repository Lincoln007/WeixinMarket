using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;

namespace Weixin.Iserivce
{
    public interface IRoleService:IserviceSupport
    {
        Task<long> AddNew(string roleName);
        Task Edit(long roleId, string roleName);
        Task Delete(long roleId);
        Task<RoleDTO> GetById(long roleId);
        Task<RoleDTO[]> GetByName(string roleName);
        Task<RoleDTO[]> GetAll();

        Task AddRoleIds(long userId, long[] roleIds); //给用户增加角色
        Task EditRoleIds(long userId, long[] roleIds); //更新用户角色，先删再加
        Task<RoleDTO[]> GetByUserId(long userId); //获取用户的角色
    }
}
