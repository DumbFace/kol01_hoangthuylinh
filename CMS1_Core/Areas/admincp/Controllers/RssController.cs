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
    public class RssController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InstantArticle(int Id)
        {
            myfunc func = new myfunc();
            tblNews tbl = func.GetContentNewsbyId(Id);
            ViewBag.lstRelated = func.GetRelatedNews(tbl.IdZone, tbl.Id);
            return View(tbl);
        }
    }
}