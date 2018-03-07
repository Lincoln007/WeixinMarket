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
        public long AddNew(string weixinName, string appid, string token, string encodingAESKey, string appsecret)
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
                    Appsecret = appsecret
                };
                var exists = service.GetAll().Any(a => a.Appid == appid);
                if (exists)
                {
                    throw new ArgumentException("该公众号appid已经存在");
                }
                db.BaseConfig.Add(config);
                db.SaveChanges();
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

        public async Task Create(string WeixinName,string Appid,string Token,string EncodingAESKey,string Appsecret,
            string DefaultResponse)
        {
            using (var db=new WeixinDbContext())
            {
                int count = await GetByAppid(Appid);
                if (count>0) //说明已经存在该appid
                {
                    throw new ArgumentException("该公众号已经存在！");
                }
                else
                {
                    var config = new BaseConfig()
                    {
                        WeixinName = WeixinName,
                        Appid = Appid,
                        Token = Token,
                        EncodingAESKey = EncodingAESKey,
                        Appsecret = Appsecret
                    };
                    db.BaseConfig.Add(config);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task Edit(long id,string WeixinName, string Token, string EncodingAESKey, string Appsecret,
            string DefaultResponse)
        {
            using (var db = new WeixinDbContext())
            {
                var commonService = new CommonService<BaseConfig>(db);
                var config = await commonService.GetById(id);
                if (config==null)
                {
                    throw new ArgumentNullException();
                }
                config.WeixinName = WeixinName;
                config.Token = Token;
                config.EncodingAESKey = EncodingAESKey;
                config.Appsecret = Appsecret;
                config.DefaultResponse = DefaultResponse;
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
