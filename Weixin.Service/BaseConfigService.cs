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
    public class BaseConfigService : IBaseConfigService
    {
        public async Task<long> AddNew(string weixinName, string appid, string token, string encodingAESKey, string appsecret,
            string defaultResponse)
        {          
            using (var db=new WeixinDbContext())
            {
                CommonService<BaseConfig> service = new CommonService<BaseConfig>(db);
                var config = new BaseConfig()
                {
                    WeixinName = weixinName,
                    Appid = appid,
                    Token = token,
                    EncodingAESKey = encodingAESKey,
                    Appsecret = appsecret,
                    DefaultResponse=defaultResponse
                };
                var exists = await service.GetAll().AnyAsync(a => a.Appid == appid);
                if (exists)
                {
                    throw new ArgumentException("该公众号appid已经存在");
                }
                db.BaseConfig.Add(config);
                await db.SaveChangesAsync();
                return config.Id;
            }
        }

        public async Task<BaseConfigDTO> GetById(long id)
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<BaseConfig> service = new CommonService<BaseConfig>(db);
                var entity = await service.GetById(id);
                return ToDTO(entity);
            }
        }

        private BaseConfigDTO ToDTO(BaseConfig entity)
        {
            BaseConfigDTO dto = new BaseConfigDTO()
            {
                Id = entity.Id,
                Appid = entity.Appid,
                Appsecret = entity.Appsecret,
                EncodingAESKey = entity.EncodingAESKey,
                Token = entity.Token,
                WeixinName = entity.WeixinName,
                DefaultResponse=entity.DefaultResponse
            };
            return dto;
        }

        public async Task<BaseConfigDTO[]> GetAll()
        {
            using (var db=new WeixinDbContext())
            {
                CommonService<BaseConfig> commonService = new CommonService<BaseConfig>(db);
                return (await commonService.GetAll().AsNoTracking().ToListAsync()).Select(a => ToDTO(a)).ToArray();
            }
        }

        public async Task Edit(long id,string weixinName, string token, string encodingAESKey, string appsecret,
            string defaultResponse)
        {
            using (var db = new WeixinDbContext())
            {
                var commonService = new CommonService<BaseConfig>(db);
                var config = await commonService.GetById(id);
                if (config==null)
                {
                    throw new ArgumentNullException();
                }
                config.WeixinName = weixinName;
                config.Token = token;
                config.EncodingAESKey = encodingAESKey;
                config.Appsecret = appsecret;
                config.DefaultResponse = defaultResponse;
                await db.SaveChangesAsync();
            }
        }

        public async Task<int> GetByAppid(string appid)
        {
            using (var db=new WeixinDbContext())
            {
                return await db.BaseConfig.CountAsync(b => b.Appid == appid);
            }
        }

        public async Task Delete(long id)
        {
            using (var db=new WeixinDbContext())
            {
                var commonService = new CommonService<BaseConfig>(db);
                var config = await commonService.GetById(id);
                if (config == null)
                {
                    throw new ArgumentNullException();
                }
                db.BaseConfig.Remove(config);
                await db.SaveChangesAsync();
            }
        }
    }
}
