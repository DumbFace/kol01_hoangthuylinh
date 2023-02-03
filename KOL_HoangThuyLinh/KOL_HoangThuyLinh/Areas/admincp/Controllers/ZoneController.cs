using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Areas.admincp.Controllers
{
    public class ZoneController : Controller
    {
        // GET: admincp/Zone
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetList()
        {
            return PartialView(new myfunc().GetListZone());
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