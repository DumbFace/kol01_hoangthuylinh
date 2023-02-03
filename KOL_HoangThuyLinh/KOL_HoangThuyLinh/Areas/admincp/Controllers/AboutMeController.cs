using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Areas.admincp.Controllers
{
    public class AboutMeController : Controller
    {

        // GET: admincp/AbouMe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAboutMe()
        {
            return View(new myfunc().GetProfile());
        }

        public ActionResult ShowUrlImage()
        {
            return PartialView();
        }

        [ValidateInput(false)]
        public ActionResult UpdateAboutMe(tblKOL tbl)
        {
            new myfunc().UpdateAboutMe(tbl);
            if (myCommon.isEnableRedisCached)
            {
                MyCacheFunc.SetAboutMe();
            }
            return Content("ok");
        }
    }
}