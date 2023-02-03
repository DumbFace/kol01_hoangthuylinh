using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Controllers
{
    public class TypeNewsController : Controller
    {
        // GET: Zone
        public ActionResult Index(string url,string id)
        {
            myfunc func = new myfunc();
            tblNewsType NewsType = myCommon.GetNewsIdType.Where(x=>x.Id == int.Parse(id)).FirstOrDefault();
            ViewBag.Title = NewsType.Name;
            ViewBag.Description = NewsType.Name;
            ViewBag.Id = int.Parse(id);
            ViewBag.currentMenu = NewsType.NameEng;
            ViewBag.TypeName = NewsType.Name;
            return View();
        }

        public ActionResult GetList(int Id, int page = 1, int pagesize = 10)
        {
            ViewBag.Id = Id;
            return PartialView(new myfunc().GetNewsByType(Id, page, pagesize));
        }
    }
}