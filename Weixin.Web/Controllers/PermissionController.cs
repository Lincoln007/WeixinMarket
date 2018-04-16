using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Weixin.Iserivce;
using Weixin.Web.Models;
using Weixin.DTO;
using Weixin.Core.Helper;

namespace Weixin.Web.Controllers
{
    public class PermissionController : Controller
    {
        public IPermissionService permissionService { get; set; }
        //[CheckPermission("Permission.List")]
        public async Task<ActionResult> Index()
        {
            int pageSize = 5; //设置每页显示个数
            int pageIndex = string.IsNullOrEmpty(Request["pageIndex"]) ? 1 : int.Parse(Request["pageIndex"]); //获得当前页，默认第一页
            int skip = (pageIndex - 1) * pageSize; //计算要跳过的条数
            var list = (await permissionService.GetAll()).OrderBy(p => p.PermissionName).Skip(skip).Take(pageSize);
            var pageCount = Math.Ceiling(list.Count() / (double)pageSize);
            ViewBag.PageBar = CommonHelper.CreatePageBar(pageIndex, (int)pageCount, "Permission", "Index");
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(PermissionCreateModel model)
        {
            long id = await permissionService.AddNew(model.PermissionName, model.Description);
            return Redirect("Index");
        }

        public async Task<ActionResult> Edit(long id)
        {
            PermissionDTO entity;
            try
            {
                entity = await permissionService.GetById(id);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            var model = new PermissionEditModel()
            {
                PermissionId = id,
                PermissionName = entity.PermissionName,
                Description = entity.Description,
                Referer = Request.UrlReferrer.AbsoluteUri
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PermissionEditModel model)
        {
            await permissionService.Edit(model.PermissionId, model.PermissionName, model.Description);
            return Redirect(model.Referer);
        }

        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await permissionService.Delete(id);
                return Redirect("Index");
            }
            catch (ArgumentException ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Search(string permissionName)
        {
            if (string.IsNullOrEmpty(permissionName))
            {
                return RedirectToAction("Index");
            }
            int pageSize = 5; //设置每页显示个数
            int pageIndex = string.IsNullOrEmpty(Request["pageIndex"]) ? 1 : int.Parse(Request["pageIndex"]); //获得当前页，默认第一页
            int skip = (pageIndex - 1) * pageSize; //计算要跳过的条数
            var list = (await permissionService.GetByName(permissionName))
                .OrderBy(p => p.PermissionName).Skip(skip).Take(pageSize);
            var pageCount = Math.Ceiling(list.Count() / (double)pageSize);
            string[] parameters = new string[] { "permissionName=" + permissionName };
            ViewBag.PageBar = CommonHelper.CreatePageBar(pageIndex, (int)pageCount, "Permission", "Index", parameters);
            return View("Index", list);
        }
    }
}