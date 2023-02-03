using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Areas.admincp.Controllers
{
    public class GalleryCPController : Controller
    {
        // GET: admincp/Gallery
        public ActionResult Gallery()
        {
            return View();
        }

        public ActionResult GetList(int page = 1, int pagesize = 10)
        {
            ViewBag.page = page;
            ViewBag.pagesize = pagesize;
            return PartialView(new myfunc().GetListGallery(page, pagesize));
        }


        public ActionResult SaveGallery(tblGallery tbl,string dtranpicker)
        {
            tbl.CreateDate = DateTime.ParseExact(dtranpicker, "yyyy/MM/dd HH:mm", null);

            if (tbl.Id == 0)
            {
                new myfunc().InsertGallery(tbl);
            }
            else
                new myfunc().UpdateGallery(tbl);
            return Content("ok");
        }

        public ActionResult AddGallery(int Id)
        {
            if (Id == 0)
            {
                return PartialView("AddGallery", new tblGallery() { Id = 0 });
            }
            else
            {
                return PartialView("AddGallery", new myfunc().GetGalleryById(Id));
            }
        }

        public ActionResult DeleteGallery(int Id)
        {
            new myfunc().DeleteGallery(Id);
            return Content("ok");
        }

        public ActionResult GalleryDetail(int id)
        {
            return View(new myfunc().GetGalleryById(id));
        }

        public ActionResult DeleteGalleryDT(int Id)
        {
            new myfunc().DeleteGalleryDT(Id);
            return Content("ok");
        }

        [HttpPost]
        public ActionResult AddGalleryDetail(List<tblGalleryDT> tbls)
        {
            if (tbls.Count > 0)
            {
                foreach (tblGalleryDT _tbl in tbls)
                {
                    tblGalleryDT tbl = new tblGalleryDT() { IdGallery = _tbl.Id, Avatar = _tbl.Avatar, CreateDate = DateTime.Now };
                    new myfunc().InsertGalleryDetail(tbl);
                }
            }
            return Content("OK");
        }

        public ActionResult DeleteCheckedDT(string ids)
        {
            string[] arrayId = ids.Split(',');
            for (int i = 0; i < arrayId.Length; i++)
            {
                new myfunc().DeleteGalleryDT(int.Parse(arrayId[i]));
            }
            return Content("OK");
        }

        public ActionResult GetListDT(int id, int page = 1, int pagesize = 10)
        {
            ViewBag.id = id;
            return PartialView(new myfunc().GetListGalleryDT(id, page, pagesize));
        }

    }
}