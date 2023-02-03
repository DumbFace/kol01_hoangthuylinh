using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Controllers
{
    public class ReadController : Controller
    {

        // GET: Read
        public ActionResult Index(string url, string id)
        {
            int contentId = -1;

            if (!string.IsNullOrEmpty(id))
                contentId = int.Parse(id);

            tblNews tbl = myCommon.isEnableRedisCached ? MyCacheFunc.GetNews(contentId) : new myfunc().GetContentNewsbyId(contentId);

            if (tbl == null)
            {
                return RedirectToAction("index", "home");
            }
            
            string sessionContent = "Page_" + contentId;
            if (Session[sessionContent] == null)
            {
                new myfunc().UpdateViewCount(contentId);
                Session[sessionContent] = 1;
            }
            string strUrl = WebConfigurationManager.AppSettings["UrlHost"];
            ViewBag.Title = tbl.Title;
            ViewBag.Description = tbl.Teaser.Replace("\"", "'");
            ViewBag.url = strUrl + tbl.UrlLink + "-" + tbl.Id.ToString() + ".html";
            ViewBag.Image = tbl.AvatarFB;
            ViewBag.Published = tbl.PublishDate;

            //Bỏ bài viết có cùng Id trong bài biết liên quan
            ViewBag.lstRelated = myCommon.isEnableRedisCached ? 
                MyCacheFunc.GetLastNews().Where(m => m.Id != contentId) : new myfunc().GetLastNewsHome(8);

            return View(tbl);
        }
    }
}