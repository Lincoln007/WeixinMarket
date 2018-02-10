using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Weixin.DTO;
using Weixin.Iserivce;
using Weixin.Web.Models;

namespace Weixin.Web.Controllers
{
    public class ConfigController : Controller
    {
        public IBaseConfigService baseConfigService { get; set; }
        public async Task<ActionResult> Index()
        {
            var list = await baseConfigService.GetAll();
            return View(list);
        }

        private async Task<BaseConfigDTO> GetConfig(long id)
        {
            var config = await baseConfigService.GetById(id);
            if (config == null)
            {
                throw new ArgumentException("未能找到该公众号配置");
            }
            return config;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(BaseConfigCreateModel model)
        {
            await baseConfigService.Create(model.WeixinName, model.Appid, model.Token, model.EncodingAESKey,
                model.Appsecret, model.DefaultResponse);
            return RedirectToAction("Index");
        }
    }
}