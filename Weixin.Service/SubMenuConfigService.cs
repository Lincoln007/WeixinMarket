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
    public class SubMenuConfigService : ISubMenuConfigService
    {
        public async Task<long> AddNew(string type, string menuName, string keyUrlMediaId, long parentMenuID, 
            int menuOrder)
        {
            var entity = new SubMenuConfig()
            {
                Type = type,
                MenuName = menuName,
                KeyUrlMediaId = keyUrlMediaId,
                ParentMenuID = parentMenuID,
                MenuOrder = menuOrder
            };
            using (var db=new WeixinDbContext())
            {
                db.SubMenuConfig.Add(entity);
                await db.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task Delete(long id)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<SubMenuConfig> commonService = new CommonService<SubMenuConfig>(db);
                var entity = await commonService.GetById(id);
                if (entity==null)
                {
                    throw new ArgumentException("未找到菜单配置");
                }
                db.SubMenuConfig.Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task Edit(long id, string type, string menuName, string keyUrlMediaId, long parentMenuID,
            int menuOrder)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<SubMenuConfig> commonService = new CommonService<SubMenuConfig>(db);
                var entity = await commonService.GetById(id);
                if (entity == null)
                {
                    throw new ArgumentException("未找到菜单配置");
                }
                entity.Type = type;
                entity.MenuName = menuName;
                entity.KeyUrlMediaId = keyUrlMediaId;
                entity.ParentMenuID = parentMenuID;
                entity.MenuOrder = menuOrder;
                await db.SaveChangesAsync();
            }
        }

        public async Task<SubMenuConfigDTO[]> GetAll(long parentMenuID)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<SubMenuConfig> commonService = new CommonService<SubMenuConfig>(db);
                var list = await commonService.GetAll().Include(s=>s.ParentMenuConfig)
                    .Where(s => s.ParentMenuID == parentMenuID).ToListAsync();
                return list.Select(s => ToDTO(s)).ToArray();
            }
        }

        private SubMenuConfigDTO ToDTO(SubMenuConfig entity)
        {
            SubMenuConfigDTO dto = new SubMenuConfigDTO()
            {
                MenuName = entity.MenuName,
                MenuOrder = entity.MenuOrder,
                Type = entity.Type,
                KeyUrlMediaId = entity.KeyUrlMediaId,
                ParentMenuID = entity.ParentMenuID,
                ParentMenuName = entity.ParentMenuConfig.MenuName
            };
            return dto;
        }
    }
}
