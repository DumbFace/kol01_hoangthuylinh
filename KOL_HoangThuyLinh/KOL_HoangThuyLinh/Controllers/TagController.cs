using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;
using MvcPaging;

namespace KOL_HoangThuyLinh.Controllers
{
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult Index(int id)
        {
            myfunc func = new myfunc();
            tblNews obj = func.GetTopTag(id);
            tblTag tbl = func.GetTagbyId(id);
            ViewBag.Title = tbl.TagName + ": " + obj.Title;
            ViewBag.Description = tbl.TagName + ": " + obj.Teaser;
            ViewBag.PubDate = obj.PublishDate;

            return View(tbl);
        }

        public ActionResult GetList(int Id, int page = 1, int pagesize = 10)
        {
            ViewBag.Id = Id;
            var lst = myCommon.isEnableRedisCached ? 
                                        MyCacheFunc.GetNewsByIdTag(Id).ToPagedList(page - 1, pagesize) :
                                        new myfunc().GetNewsByTag(Id, page, pagesize);
            return PartialView(lst);
        }
    }
}