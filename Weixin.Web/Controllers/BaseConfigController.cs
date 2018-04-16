using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Weixin.Core.Helper;
using Weixin.DTO;
using Weixin.Iserivce;
using Weixin.Web.Models;

namespace Weixin.Web.Controllers
{
    public class BaseConfigController : Controller
    {
        public IBaseConfigService baseConfigService { get; set; }
        public async Task<ActionResult> Index()
        {
            int pageSize = 5; //设置每页显示个数
            int pageIndex = string.IsNullOrEmpty(Request["pageIndex"]) ? 1 : int.Parse(Request["pageIndex"]); //获得当前页，默认第一页
            int skip = (pageIndex - 1) * pageSize; //计算要跳过的条数
            var list = (await baseConfigService.GetAll()).OrderBy(p => p.Id).Skip(skip).Take(pageSize);
            var pageCount = Math.Ceiling(list.Count() / (double)pageSize);
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
        public ActionResult Create(BaseConfigCreateModel model)
        {
            var id = baseConfigService.AddNew(model.WeixinName, model.Appid, model.Token, model.EncodingAESKey,
                model.Appsecret, model.DefaultResponse);
            Senparc.Weixin.Threads.ThreadUtility.Register(); //创建公众号的同时进行AccessToken的注册
            Senparc.Weixin.MP.Containers.AccessTokenContainer.Register(model.Appid, model.Appsecret, model.WeixinName);
            return Redirect("/BaseConfig/Index");
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id==null)
            {
                throw new ArgumentNullException();
            }
            var config = await baseConfigService.GetById(id.Value);
            if (config==null)
            {
                throw new ArgumentNullException();
            }
            return View(config);
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(BaseConfigEditModel model)
        {
            await baseConfigService.Edit(model.Id, model.WeixinName, model.Token, model.EncodingAESKey, model.Appsecret,
                model.DefaultResponse);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            await baseConfigService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Search(string weixinName)
        {
            if (string.IsNullOrEmpty(weixinName))
            {
                return RedirectToAction("Index");
            }
            int pageSize = 5; //设置每页显示个数
            int pageIndex = string.IsNullOrEmpty(Request["pageIndex"]) ? 1 : int.Parse(Request["pageIndex"]); //获得当前页，默认第一页
            int skip = (pageIndex - 1) * pageSize; //计算要跳过的条数
            var list = (await baseConfigService.GetByName(weixinName))
                .OrderBy(p => p.Id).Skip(skip).Take(pageSize);
            var pageCount = Math.Ceiling(list.Count() / (double)pageSize);
            string[] parameters = new string[] { "roleName=" + weixinName };
            ViewBag.PageBar = CommonHelper.CreatePageBar(pageIndex, (int)pageCount, "Role", "Index", parameters);
            return View("Index", list);
        }
    }
}