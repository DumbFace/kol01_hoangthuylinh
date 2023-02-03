using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CMS1_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMS1_Core.Areas.admincp.Controllers
{
    [Area("Admincp")]
    public class ZoneController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(int page = 1, int pagesize = 10)
        {
            return PartialView(new myfunc().GetListZone(page,pagesize));
        }

        public ActionResult SaveZone(tblZone tbl)
        {
            if (tbl.Id == 0)
                new myfunc().InsertZone(tbl.ZoneName);
            else
                new myfunc().UpdateZone(tbl);

            return Content("ok");
        }

        public ActionResult AddZone(int Id)
        {
            if (Id == 0)
            {
                return PartialView(new tblZone() { Id = 0 });
            }
            else
            {
                return PartialView(new myfunc().GetZonebyId(Id));
            }
        }

        public ActionResult DeleteZone(int Id)
        {
            new myfunc().DeleteZone(Id);
            return Content("ok");
        }
    }
}