using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CMS1_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMS1_Core.Areas.admincp.Controllers
{
    [Area("Admincp")]
    public class ContentController : BaseController
    {
        private int limitPageSize = Helpers.Constants.limitContent;
        // GET: admincp/Content
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View(new tblNews());
        }

        public ActionResult Update(int Id)
        {
            tblNews tbl = new myfunc().GetNewsbyId(Id);
            ViewBag.lstTag = tbl.Tags;
            return View(tbl);
        }

        public ActionResult SaveNews(tblNews tbl, int intstatus = 0, int intFlag = 0, string dtPublic = "", string tags = "")
        {
            tblUser obj = GetLoggedUser;

            tbl.Title = myCommon.RemoveSpecial(tbl.Title);
            tbl.Teaser = myCommon.RemoveSpecial(tbl.Teaser);
            tbl.UrlLink = myCommon.FriendlyUrl(tbl.Title);
            tbl.Status = intstatus;
            tbl.IdUser = obj.Id;
            tbl.PublishDate = DateTime.ParseExact(dtPublic, "yyyy/MM/dd HH:mm", null);

            int Id = 0;
            myfunc myfunc = new myfunc();

            if (intFlag == 0)
            {
                Id = myfunc.InsertNews(tbl);
            }
            else
            {
                Id = tbl.Id;

                myfunc.UpdateNews(tbl);
                myfunc.DeleteNewsTags(Id);
            }

            if (!string.IsNullOrEmpty(tags))
            {
                myfunc.InsertNewsTags(Id, tags);
            }
            return Content("ok");
        }
        [MyAuthorize (Roles = "Admin")]
        public ActionResult ListActive()
        {
            return View();
        }

        public ActionResult GetListActive(string key, string fromdate, string todate, string idzone, int page = 1, int pagesize = 5)
        {
            ViewBag.page = page;
            ViewBag.pagesize = pagesize;
            ViewBag.key = key;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.idzone = idzone;

            return PartialView(new myfunc().FilterContent(fromdate, todate, key, 1, idzone, page, pagesize));
        }
        public ActionResult GetListDelay(string key, string fromdate, string todate, string idzone, int page = 1, int pagesize = 5)
        {
            ViewBag.page = page;
            ViewBag.pagesize = pagesize;
            ViewBag.key = key;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.idzone = idzone;
            return PartialView(new myfunc().FilterContent(fromdate, todate, key, 0, idzone, page, pagesize));
        }

        public ActionResult ListDelay()
        {
            return View();
        }

        public ActionResult UpdateStatus(int Id)
        {
            new myfunc().UpdateStatus(Id);
            return Content("ok");
        }

        public ActionResult DeleteNews(int Id)
        {
            new myfunc().DeleteNews(Id);
            return Content("ok");
        }

        public ActionResult ShowUrlImage(int Id)
        {
            ViewBag.IdType = Id;
            return PartialView();
        }
    }
}