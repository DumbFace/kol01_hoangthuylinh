using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;

namespace kol01_hoangthuylinh.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {
            ViewBag.currentMenu = "VIDEO";
            tblConfig tbl =  new myfunc().GetConfigWeb();
            ViewBag.Title = "Video nghệ sĩ Hoàng Thùy Linh" ;
            ViewBag.Description = tbl.DescriptionWeb;
            return View();
        }

        public ActionResult GetList(int page= 1, int pagesize = 12) 
        {
            return PartialView(new myfunc().GetVideoCategory(page,pagesize));
        }
    }
}