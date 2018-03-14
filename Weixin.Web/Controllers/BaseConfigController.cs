﻿using System;
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
    public class BaseConfigController : Controller
    {
        public IBaseConfigService BaseConfigService { get; set; }
        public async Task<ActionResult> Index()
        {
            var list = await BaseConfigService.GetAll();
            return View(list);
        }

        private async Task<BaseConfigDTO> GetConfig(long id)
        {
            var config = await BaseConfigService.GetById(id);
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
        public ActionResult Create(BaseConfigCreateModel model)
        {
            var id = BaseConfigService.AddNew(model.WeixinName, model.Appid, model.Token, model.EncodingAESKey,
                model.Appsecret, model.DefaultResponse);
            Senparc.Weixin.Threads.ThreadUtility.Register(); //创建公众号的同时进行AccessToken的注册
            Senparc.Weixin.MP.Containers.AccessTokenContainer.Register(model.Appid, model.Appsecret, model.WeixinName);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id==null)
            {
                throw new ArgumentNullException();
            }
            var config = await BaseConfigService.GetById(id.Value);
            if (config==null)
            {
                throw new ArgumentNullException();
            }
            return View(config);
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(BaseConfigEditModel model)
        {
            await BaseConfigService.Edit(model.Id, model.WeixinName, model.Token, model.EncodingAESKey, model.Appsecret,
                model.DefaultResponse);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            await BaseConfigService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}