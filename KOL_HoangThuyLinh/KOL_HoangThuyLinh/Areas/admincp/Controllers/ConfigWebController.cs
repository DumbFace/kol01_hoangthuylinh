using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Areas.admincp.Controllers
{
    public class ConfigWebController : Controller
    {
        public ActionResult Index()
        {
            return View(new myfunc().GetConfigWeb());
        }

        public ActionResult UpdateConfig(tblConfig tbl)
        {
            new myfunc().UpdateConfigWeb(tbl);
            if (myCommon.isEnableRedisCached)
            {
                MyCacheFunc.SetConfigWeb();
            }
            return Content("ok");
        }
    }
}