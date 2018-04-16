using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weixin.DTO;
using Weixin.Iserivce;
using Weixin.Service.Entities;

namespace Weixin.Service
{
    public class RoleService : IRoleService
    {
        public async Task<long> AddNew(string roleName)
        {
            using (var db = new WeixinDbContext())
            {
                var role = new Role() { RoleName = roleName };
                db.Role.Add(role);
                await db.SaveChangesAsync();
                return role.Id;
            }
        }

        public async Task AddRoleIds(long userId, long[] roleIds)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<User> userCs = new CommonService<User>(db);
                var user = await userCs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户不存在" + userId);
                }
                CommonService<Role> roleCs = new CommonService<Role>(db);

                //寻找数据库里的roleId和传入的roleId的交集
                var roles = roleCs.GetAll().Where(r => roleIds.Contains(r.Id)).ToArray();
                foreach (var role in roles)
                {
                    user.Roles.Add(role);
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(long roleId)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(db);
                var role = await commonService.GetById(roleId);
                if (role == null)
                {
                    throw new ArgumentException("该角色不存在！");
                }
                db.Role.Remove(role);
                await db.SaveChangesAsync();
            }
        }

        public async Task Edit(long roleId, string roleName)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(db);
                var role = await commonService.GetById(roleId);
                if (role == null)
                {
                    throw new ArgumentException("该角色不存在！");
                }
                role.RoleName = roleName;
                await db.SaveChangesAsync();
            }
        }

        public async Task EditRoleIds(long userId, long[] roleIds)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<User> userCs = new CommonService<User>(db);
                var user = await userCs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户不存在" + userId);
                }
                user.Roles.Clear();//先清空用户的角色

                CommonService<Role> roleCs = new CommonService<Role>(db);
                //寻找数据库里的roleId和传入的roleId的交集
                var roles = await roleCs.GetAll().Where(r => roleIds.Contains(r.Id)).ToArrayAsync();
                foreach (var role in roles)
                {
                    user.Roles.Add(role);//重新添加角色
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task<RoleDTO[]> GetAll()
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(db);
                var all = await commonService.GetAll().ToListAsync();
                return all.Select(a => ToDTO(a)).ToArray();
            }
        }

        public async Task<RoleDTO> GetById(long roleId)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(db);
                var role = await commonService.GetById(roleId);
                if (role==null)
                {
                    throw new ArgumentException("角色未找到");
                }
                return ToDTO(role);
            }
        }

        public async Task<RoleDTO[]> GetByName(string roleName)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(db);
                var roles = commonService.GetAll().Where(r => r.RoleName.Contains(roleName));
                var list = await roles.ToListAsync();
                return list.Select(r => ToDTO(r)).ToArray();
            }
        }

        public async Task<RoleDTO[]> GetByUserId(long userId)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<User> commonService = new CommonService<User>(db);
                var user = await commonService.GetById(userId);
                if (user==null)
                {
                    throw new ArgumentException("用户未找到");
                }
                return user.Roles.ToList().Select(r => ToDTO(r)).ToArray();
            }
        }

        private RoleDTO ToDTO(Role role)
        {
            var dto = new RoleDTO()
            {
                RoleName = role.RoleName,
                Id = role.Id
            };
            return dto;
        }
    }
}
