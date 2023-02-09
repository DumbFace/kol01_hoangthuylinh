using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace kol01_hoangthuylinh.Controllers
{
    
    public class TagController : Controller
    {

        // Test 
        [Route("/tag/{url}-{id}.html")]
        public IActionResult Index(int id)
        {
            myfunc func = new myfunc();
            tblNews obj = func.GetTopTag(id);
            tblTag tbl = func.GetTagbyId(id);
            ViewBag.Title = tbl.TagName + ": " + obj.Title;
            ViewBag.Description = tbl.TagName + ": " + obj.Teaser;
            ViewBag.PubDate = obj.PublishDate;
            ViewBag.showHead = 0;
            return View(tbl);
        }

        public IActionResult GetList(int Id, int page = 1, int pagesize = 10)
        {
            ViewBag.page = page;
            ViewBag.pagesize = pagesize;
            ViewBag.Id = Id;
            return PartialView(new myfunc().GetNewsByTag(Id,page,pagesize));
        }
    }
}