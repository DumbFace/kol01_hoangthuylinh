using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;

namespace kol01_hoangthuylinh.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            ViewBag.currentMenu = "NEWS";
            tblConfig tbl = new myfunc().GetConfigWeb();
            ViewBag.Title = "Tin tức nghệ sĩ Hoàng Thùy Linh";
            ViewBag.Description = tbl.DescriptionWeb;
            return View();
        }

        public ActionResult GetList(int page = 1, int pagesize = 12)
        {
            return PartialView(new myfunc().GetNewsCategory(page, pagesize));
        }
    }
}