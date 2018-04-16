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
    public class RoleController : Controller
    {      
        public IRoleService roleService { get; set; }
        public async Task<ActionResult> Index()
        {
            int pageSize = 5; //设置每页显示个数
            int pageIndex = string.IsNullOrEmpty(Request["pageIndex"]) ? 1 : int.Parse(Request["pageIndex"]); //获得当前页，默认第一页
            int skip = (pageIndex - 1) * pageSize; //计算要跳过的条数
            var list = (await roleService.GetAll()).OrderBy(p => p.RoleName).Skip(skip).Take(pageSize);
            var pageCount = Math.Ceiling(list.Count() / (double)pageSize);
            ViewBag.PageBar = CommonHelper.CreatePageBar(pageIndex, (int)pageCount, "Role", "Index");
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string roleName)
        {
            long id = await roleService.AddNew(roleName);
            return Redirect("Index");
        }

        public async Task<ActionResult> Edit(long id)
        {
            var referer = Request.UrlReferrer.AbsoluteUri;
            RoleEditModel model = new RoleEditModel();
            model.Referer = referer;
            try
            {
                var entity = await roleService.GetById(id);
                model.Id = entity.Id;
                model.RoleName = entity.RoleName;
                return View(model);
            }
            catch (ArgumentException ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleEditModel model)
        {
            try
            {
                await roleService.Edit(model.Id, model.RoleName);
                return Redirect(model.Referer);
            }
            catch (ArgumentException ex)
            {
                return Content(ex.Message);
            }
        }

        public async Task<ActionResult> Delete(long id)
        {
            try
            { 
                await roleService.Delete(id);
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }
            catch (ArgumentException ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Search(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return RedirectToAction("Index");
            }
            int pageSize = 5; //设置每页显示个数
            int pageIndex = string.IsNullOrEmpty(Request["pageIndex"]) ? 1 : int.Parse(Request["pageIndex"]); //获得当前页，默认第一页
            int skip = (pageIndex - 1) * pageSize; //计算要跳过的条数
            var list = (await roleService.GetByName(roleName))
                .OrderBy(p => p.RoleName).Skip(skip).Take(pageSize);
            var pageCount = Math.Ceiling(list.Count() / (double)pageSize);
            string[] parameters = new string[] { "roleName=" + roleName };
            ViewBag.PageBar = CommonHelper.CreatePageBar(pageIndex, (int)pageCount, "Role", "Index", parameters);
            return View("Index", list);
        }
    }
}