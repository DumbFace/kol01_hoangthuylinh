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
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            string url = WebConfigurationManager.AppSettings["UrlHost"];
            ViewBag.url = url;
            ViewBag.Image = url + "Content/Images/logohome.jpg";
            ViewData["aboutMe"] = myCommon.isEnableRedisCached ? MyCacheFunc.GetAboutMe() : new myfunc().GetProfile();
            ViewBag.Title = myCommon.isEnableRedisCached ? MyCacheFunc.GetConfigWeb().TitleWeb : new myfunc().GetConfigWeb().TitleWeb;

            return View();
        }

        public ActionResult GetLatesNews()
        {
            List<tblHome> lst = myCommon.isEnableRedisCached ? MyCacheFunc.GetLastNews() : new myfunc().GetLastNewsHome(8);
            return PartialView(lst);
        }

        public ActionResult GetList(int page = 1, int pagesize = 12)
        {
            IPagedList<tblHome> lst = myCommon.isEnableRedisCached ?
                                     MyCacheFunc.GetNewsHome().ToPagedList(page - 1, pagesize) :
                                     new myfunc().GetNewsHome(page, pagesize);
            return PartialView(lst);
        }

        public ActionResult GetTagsView()
        {
            List<tblTag> lst = myCommon.isEnableRedisCached ? MyCacheFunc.GetTags() : new myfunc().GetListTagView(10);
            return PartialView(lst);
        }

    }
}