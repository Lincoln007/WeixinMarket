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
    public class ParentMenuConfigService : IParentMenuConfigService
    {
        public async Task<long> AddNew(string menuName, int menuOrder, int weixinId)
        {
            using (var db=new WeixinDbContext())
            {
                var entity = new ParentMenuConfig()
                {
                    MenuName = menuName,
                    MenuOrder = menuOrder,
                    WeixinID = weixinId
                };
                db.ParentMenuConfig.Add(entity);
                await db.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task Delete(long id)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<ParentMenuConfig> commonService = new CommonService<ParentMenuConfig>(db);
                var entity = await commonService.GetById(id);
                if (entity==null)
                {
                    throw new ArgumentException("未找到该菜单配置");
                }
                db.ParentMenuConfig.Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task Edit(long id, string menuName, int menuOrder, int weixinId)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<ParentMenuConfig> commonService = new CommonService<ParentMenuConfig>(db);
                var entity = await commonService.GetById(id);
                if (entity == null)
                {
                    throw new ArgumentException("未找到该菜单配置");
                }
                entity.MenuName = menuName;
                entity.MenuOrder = menuOrder;
                entity.WeixinID = weixinId;
                await db.SaveChangesAsync();
            }
        }

        //该公众号的所有父菜单配置
        public async Task<ParentMenuConfigDTO[]> GetAll(int weixinId)
        {
            using (var db = new WeixinDbContext())
            {
                CommonService<ParentMenuConfig> commonService = new CommonService<ParentMenuConfig>(db);
                var list = await commonService.GetAll().Where(p => p.WeixinID == weixinId).ToListAsync();
                return list.Select(p => ToDTO(p)).ToArray();
            }
        }

        private ParentMenuConfigDTO ToDTO(ParentMenuConfig entity)
        {
            ParentMenuConfigDTO dto = new ParentMenuConfigDTO()
            {
                MenuName = entity.MenuName,
                MenuOrder = entity.MenuOrder
            };
            return dto;
        }
    }
}
