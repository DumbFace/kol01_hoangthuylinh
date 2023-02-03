using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Controllers
{
    public class RssController : Controller
    {
        // GET: Rss
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize]
        public ActionResult InstantArticle(int Id)
        {
            myfunc func = new myfunc();
            tblNews tbl = func.GetContentNewsbyId(Id);
            ViewBag.lstRelated = func.GetRelatedNews(tbl.IdZone, tbl.Id);
            return View(tbl);
        }
    }
}