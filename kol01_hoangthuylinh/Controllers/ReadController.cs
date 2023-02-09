using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;
using kol01_hoangthuylinh.Helpers;
namespace kol01_hoangthuylinh.Controllers
{
    public class ReadController : Controller
    {
        private IHttpContextAccessor _httpContext;

        public ReadController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        // GET: Read
        public ActionResult Index(string url, string id)
        {
            int contentId = -1;

            if (!string.IsNullOrEmpty(id))
                contentId = int.Parse(id);

            tblNews tbl = new myfunc().GetContentNewsbyId(contentId);

            if (tbl == null)
            {
                return RedirectToAction("index", "home");
            }

            string sessionContent = "Page_" + contentId;
            if (_httpContext.HttpContext.Session.GetString(sessionContent) == null)
            {
                new myfunc().UpdateViewCount(contentId);
                _httpContext.HttpContext.Session.SetInt32(sessionContent, 1);
            }
            string strUrl = Constants.urlHost;
            ViewBag.Title = tbl.Title;
            ViewBag.Description = tbl.Teaser.Replace("\"", "'");
            ViewBag.url = strUrl + tbl.UrlLink + "-" + tbl.Id.ToString() + ".html";
            ViewBag.Image = tbl.AvatarFB;
            ViewBag.Published = tbl.PublishDate;

            //Bỏ bài viết có cùng Id trong bài biết liên quan
            ViewBag.lstRelated = new myfunc().GetLastNewsHome(8);

            return View(tbl);
        }
    }
}