using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {
            ViewBag.currentMenu = "VIDEO";
            tblConfig tbl = myCommon.isEnableRedisCached ? MyCacheFunc.GetConfigWeb() : new myfunc().GetConfigWeb();
            ViewBag.Title = "Video nghệ sĩ Hoàng Thùy Linh" ;
            ViewBag.Description = tbl.DescriptionWeb;
            return View();
        }

        public ActionResult GetList(int page= 1, int pagesize = 15) 
        {
            return PartialView(new myfunc().GetVideoCategory(page,pagesize));
        }
    }
}