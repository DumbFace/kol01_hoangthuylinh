using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace kol01_hoangthuylinh.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(string name = "")
        {
            ViewBag.key = name;
            return View();
        }

        public ActionResult GetList(string key, int page = 1, int pagesize = 10)
        {
            ViewBag.key = key;
            return PartialView(new myfunc().SearchNewsbyKey(key, page, pagesize));
        }
    }
}