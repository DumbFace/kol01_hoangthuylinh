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
    public class TagsController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult GetList(int page = 1, int pagesize = 10)
        {
            ViewBag.page = page;
            return PartialView(new myfunc().GetListTag(page, pagesize));
        }

        public ActionResult SaveTag(tblTag tbl)
        {
            int Id = tbl.Id;

            if (tbl.Id == 0)
            {
                Id = new myfunc().InsertTag(tbl);
            }
            else
            {
                new myfunc().UpdateTag(tbl);
            }

            return Content("ok");
        }

        public ActionResult AddTag(int Id)
        {
            if (Id == 0)
            {
                return PartialView("AddTag", new tblTag() { Id = 0 });
            }
            else
            {
                return PartialView("AddTag", new myfunc().GetTagbyId(Id));
            }
        }

        public ActionResult DeleteTag(int Id)
        {
            new myfunc().DeleteTag(Id);
            return Content("ok");
        }

        public ActionResult GetListTag(string q = "")
        {
            return Json(new myfunc().GetListTags(q));
        }
    }
}