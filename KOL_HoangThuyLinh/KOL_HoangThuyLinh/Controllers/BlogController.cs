using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Controllers
{
    public class BlogController : Controller
    {

        // GET: Blog
        public ActionResult Index()
        {
            tblConfig tbl = myCommon.isEnableRedisCached ? MyCacheFunc.GetConfigWeb() : new myfunc().GetConfigWeb();
            ViewBag.Title = tbl.TitleWeb;
            ViewBag.Description = tbl.DescriptionWeb;
            return View();
        }


        public ActionResult GetList(int page = 1,int pagesize = 10)
        {
            IPagedList<tblHome> lst = myCommon.isEnableRedisCached ? 
                MyCacheFunc.GetNewsHome().ToPagedList(page - 1, pagesize) : new myfunc().GetNewsHome(page, pagesize);

            return PartialView(lst);
        }
    }
}