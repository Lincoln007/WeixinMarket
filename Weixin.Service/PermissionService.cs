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
    public class PermissionService : IPermissionService
    {
        public async Task<long> AddNew(string permissionName, string description)
        {
            using (var db=new WeixinDbContext())
            {
                var entity = new Permission()
                {
                    PermissionName = permissionName,
                    Description = description
                };
                CommonService<Permission> commonService = new CommonService<Permission>(db);
                var exists = await commonService.GetAll().AnyAsync(p => p.PermissionName == permissionName);
                if (exists)
                {
                    throw new ArgumentException("改权限项已经存在");
                }
                db.Permission.Add(entity);
                await db.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task AddPermissonIds(long roleId, long[] permissionIds)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Role> roleCs = new CommonService<Role>(db);
                var role = await roleCs.GetById(roleId);
                if (role==null)
                {
                    throw new ArgumentException("该角色不存在");
                }
                CommonService<Permission> permissionCs = new CommonService<Permission>(db);
                var permissions = await permissionCs.GetAll().
                    Where(p => permissionIds.Contains(p.Id)).ToArrayAsync();
                foreach (var item in permissions)
                {
                    role.Permissions.Add(item);
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(long id)
        {
            using(var db=new WeixinDbContext())
            {
                CommonService<Permission> commonService = new CommonService<Permission>(db);
                var entity = await commonService.GetById(id);
                if (entity==null)
                {
                    throw new ArgumentException("该权限项不存在");
                }
                db.Permission.Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task Edit(long id, string permissionName, string description)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Permission> commonService = new CommonService<Permission>(db);
                var entity = await commonService.GetById(id);
                if (entity == null)
                {
                    throw new ArgumentException("该权限项不存在");
                }
                entity.Description = description;
                entity.PermissionName = permissionName;
                await db.SaveChangesAsync();
            }
        }

        public async Task EditPermissonIds(long roleId, long[] permissionIds)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Role> roleCs = new CommonService<Role>(db);
                var role = await roleCs.GetById(roleId);
                if (role == null)
                {
                    throw new ArgumentException("该角色不存在");
                }
                role.Permissions.Clear();
                CommonService<Permission> permissionCs = new CommonService<Permission>(db);
                var permissions = await permissionCs.GetAll().
                    Where(p => permissionIds.Contains(p.Id)).ToArrayAsync();
                foreach (var item in permissions)
                {
                    role.Permissions.Add(item);
                }
                await db.SaveChangesAsync();
            }
        }

        private PermissionDTO ToDTO(Permission entity)
        {
            PermissionDTO dto = new PermissionDTO()
            {
                PermissionName = entity.PermissionName,
                Description = entity.Description
            };
            return dto;
        }

        public async Task<PermissionDTO[]> GetAll()
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<Permission> commonService = new CommonService<Permission>(db);
                var perms = await commonService.GetAll().ToListAsync();
                return perms.Select(p => ToDTO(p)).ToArray();
            }
        }

        public async Task<PermissionDTO> GetById(long id)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Permission> commonService = new CommonService<Permission>(db);
                var perm = await commonService.GetById(id);
                return perm == null ? null : ToDTO(perm);
            }
        }

        public async Task<PermissionDTO> GetByName(string permissionName)
        {
            using (var db = new WeixinDbContext())
            {
                var perm = await db.Permission.SingleOrDefaultAsync(p => p.PermissionName == permissionName);
                return perm == null ? null : ToDTO(perm);
            }
        }

        public async Task<PermissionDTO[]> GetByRoleId(long roleId)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<Role> commonService = new CommonService<Role>(db);
                var role = await commonService.GetById(roleId);
                if (role==null)
                {
                    throw new ArgumentException("角色不存在");
                }
                return role.Permissions.ToList().Select(p => ToDTO(p)).ToArray();
            }
        }
    }
}
