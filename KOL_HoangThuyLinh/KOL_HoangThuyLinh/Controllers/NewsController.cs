using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            ViewBag.currentMenu = "NEWS";
            tblConfig tbl = myCommon.isEnableRedisCached ? MyCacheFunc.GetConfigWeb() : new myfunc().GetConfigWeb();
            ViewBag.Title = "Tin tức nghệ sĩ Hoàng Thùy Linh";
            ViewBag.Description = tbl.DescriptionWeb;
            return View();
        }

        public ActionResult GetList(int page = 1, int pagesize = 15)
        {
            return PartialView(new myfunc().GetNewsCategory(page, pagesize));
        }
    }
}