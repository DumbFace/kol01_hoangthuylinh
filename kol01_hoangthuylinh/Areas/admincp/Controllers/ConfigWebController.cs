using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace kol01_hoangthuylinh.Areas.admincp.Controllers
{
    [Area("Admincp")]
    [MyAuthorize(Roles = "Admin")]
    public class ConfigWebController : Controller
    {
        public ActionResult Index()
        {
            return View(new myfunc().GetConfigWeb());
        }


        public ActionResult UpdateConfig(tblConfig tbl)
        {
            new myfunc().UpdateConfigWeb(tbl);
            return Content("ok");
        }
    }
}