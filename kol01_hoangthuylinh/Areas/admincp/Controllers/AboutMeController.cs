using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;

namespace kol01_hoangthuylinh.Areas.admincp.Controllers
{
    public class AboutMeController : BaseController
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

        public ActionResult UpdateAboutMe(tblKOL tbl)
        {
            new myfunc().UpdateAboutMe(tbl);
            return Content("ok");
        }
    }
}