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
    public class HandlerConfigService : IHandlerConfigService
    {
        public async Task<long> AddNew(string appId, string className, string handleName)
        {
            using(var db=new WeixinDbContext())
            {
                CommonService<HandlerConfig> service = new CommonService<HandlerConfig>(db);
                var handelConfig = new HandlerConfig()
                {
                    AppId = appId,
                    ClassName = className,
                    HandlerName = handleName
                };
                var exists = await service.GetAll().AnyAsync(s => s.AppId == appId);
                if (exists)
                {
                    throw new ArgumentException("该公众号appid已经存在");
                }
                db.HandlerConfig.Add(handelConfig);
                await db.SaveChangesAsync();
                return handelConfig.Id;
            }
        }

        public async Task Delete(long id)
        {
            using(var db=new WeixinDbContext())
            {
                CommonService<HandlerConfig> service = new CommonService<HandlerConfig>(db);
                var handleConfig = await service.GetById(id);
                if (handleConfig == null)
                {
                    throw new ArgumentException("该公众号不存在");
                }
                db.HandlerConfig.Remove(handleConfig);
                await db.SaveChangesAsync();
            }
         
        }

        public async Task Edit(long id, string appId, string className, string handlerName)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<HandlerConfig> service = new CommonService<HandlerConfig>(db);
                var handleConfig = await service.GetById(id);
                if (handleConfig == null)
                {
                    throw new ArgumentException("该公众号不存在");
                }
                handleConfig.AppId = appId;
                handleConfig.ClassName = className;
                handleConfig.HandlerName = handlerName;
                await db.SaveChangesAsync();
            }
        }

        public async Task<HandlerConfigDTO[]> GetAll()
        {
            using (var db=new WeixinDbContext())
            {
                var service = new CommonService<HandlerConfig>(db);
                return (await service.GetAll().AsNoTracking().ToListAsync()).Select(h => ToDTO(h)).ToArray();
            }
        }

        public async Task<HandlerConfigDTO> GetById(long id)
        {
            using (var db = new WeixinDbContext())
            {
                var service = new CommonService<HandlerConfig>(db);
                var entity = await service.GetById(id);
                return ToDTO(entity);
            }
        }

        private HandlerConfigDTO ToDTO(HandlerConfig handlerConfig)
        {
            var dto = new HandlerConfigDTO()
            {
                AppId = handlerConfig.AppId,
                ClassName = handlerConfig.ClassName,
                HandlerName = handlerConfig.HandlerName
            };
            return dto;
        }
    }
}
