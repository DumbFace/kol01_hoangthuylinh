using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using kol01_hoangthuylinh.Areas.admincp.Controllers;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;

namespace kol01_hoangthuylinh.Controllers
{
    public class GalleryController : Controller
    {

        // GET: Gallery
        public ActionResult Index()
        {
            ViewBag.currentMenu = "GALLERY";
            tblConfig tbl = new myfunc().GetConfigWeb();
            ViewBag.Title = "Hình ảnh nghệ sĩ Hoàng Thùy Linh";
            ViewBag.Description = tbl.DescriptionWeb;
            return View();
        }

        public ActionResult GalleryDT(int id)
        {
            return View(new myfunc().GetListGalleryDT(id));
        }

        public ActionResult GalleryAlbum(string url, int id, int page = 1, int pagesize = 12)
        {
            ViewBag.currentMenu = "GALLERY";
            ViewBag.idGallery = id;
            ViewData["AllAlbums"] = new myfunc().GetAllAlbumById(id);
            return View(new myfunc().GetListGalleryDT(id, page,pagesize));
        }

        public ActionResult GetList(int page = 1, int pagesize = 6)
        {
            return PartialView(new myfunc().GetGalleryCategory(page, pagesize));
        }

        public ActionResult GetListAlbum(int idGallery, int page = 1, int pagesize = 12)
        {
            ViewBag.idGallery = idGallery;
            return PartialView(new myfunc().GetListGalleryDT(idGallery, page, pagesize));
        }
    }
}