using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Areas.admincp.Controllers
{
    public class uploadsController : Controller
    {

        [HttpPost]
        // GET: admincp/uploads
        public JsonResult SaveImageContent(HttpPostedFileBase fileData)
        {
            fileData = Request.Files[0];
            try
            {
                string strName = Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName);
                string strPhysical = HttpContext.Request.PhysicalApplicationPath + "uploads\\";
                string strUrl = Path.Combine(strPhysical, strName);
                fileData.SaveAs(strUrl);
                return Json(new { status = true, fileName = strName });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, fileName = ex.Message });
            }
        }


        [HttpPost]
        // GET: admincp/uploads
        public JsonResult SaveImageGallery(HttpPostedFileBase fileData)
        {
            fileData = Request.Files[0];
            try
            {
                string strName = Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName);
                string strPhysical = HttpContext.Request.PhysicalApplicationPath + "uploads\\";
                string strUrl = Path.Combine(strPhysical, strName);
                fileData.SaveAs(strUrl);
                return Json(new { status = true, fileName = strName });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, fileName = ex.Message });
            }
        }

        [HttpPost]
        // GET: admincp/uploads
        public JsonResult SaveImageGallerys()
        {
            List<tblGalleryDT> lst = new List<tblGalleryDT>();
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string strName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/uploads"), strName);
                    file.SaveAs(path);

                    tblGalleryDT tbl = new tblGalleryDT() { Avatar = strName };
                    lst.Add(tbl);
                }
            }
            return Json(new { status = true, tbl = lst });
        }

        [HttpPost]
        // GET: admincp/uploads
        public JsonResult SaveImageEditor(HttpPostedFileBase fileData)
        {
            fileData = Request.Files[0];
            try
            {
                string strName = Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName);
                string strPhysical = HttpContext.Request.PhysicalApplicationPath + "uploads\\Content\\";
                string strUrl = Path.Combine(strPhysical, strName);
                fileData.SaveAs(strUrl);
                return Json(new { status = true, fileName = strName });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, fileName = ex.Message });
            }
        }

        [HttpPost]
        // GET: admincp/uploads
        public JsonResult SaveImageAvatarThumb(HttpPostedFileBase fileData, int positionImage)
        {
            //Kiểm tra kích thước
            fileData = Request.Files[0];
            Image img = Image.FromStream(fileData.InputStream);
            int intHeight = img.Height;
            int intWidth = img.Width;
            switch (positionImage)
            {
                case 2:
                    if (intWidth != 800 || intHeight != 400)
                        return Json(new { status = "ErrorDimension", message = "Bạn vui lòng chọn kích thước 800x400" });
                    break;
                case 3:
                    if (intWidth != 400 || intHeight != 800)
                        return Json(new { status = "ErrorDimension", message = "Bạn vui lòng chọn kích thước 400x800" });
                    break;
                case 4:
                    if (intWidth != 800 || intHeight != 800)
                        return Json(new { status = "ErrorDimension", message = "Bạn vui lòng chọn kích thước 800x800" });
                    break;
            }
            try
            {
                string strName = Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName);
                string strPhysical = HttpContext.Request.PhysicalApplicationPath + "uploads\\";
                string strUrl = Path.Combine(strPhysical, strName);
                fileData.SaveAs(strUrl);
                return Json(new { status = true, fileName = strName });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, fileName = ex.Message });
            }
        }

        public JsonResult SaveThumb(string strName, int intCropX, int intCropY, int intWidth, int intHeight)
        {
            try
            {
                string strPhysical = HttpContext.Request.PhysicalApplicationPath + "uploads\\";
                string strPrefix = "1x1_" + strName;
                using (Image image = Image.FromFile(Path.Combine(strPhysical, strName)))
                {
                    using (Bitmap bmp = new Bitmap(intWidth, intHeight))
                    {
                        bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                        using (Graphics Graphic = Graphics.FromImage(bmp))
                        {
                            Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            Graphic.DrawImage(image, new Rectangle(0, 0, intWidth, intHeight), intCropX, intCropY, intWidth, intHeight, GraphicsUnit.Pixel);
                            MemoryStream ms = new MemoryStream();
                            bmp.Save(ms, image.RawFormat);
                            Image imgResult = Image.FromStream(new MemoryStream(ms.ToArray()));
                            imgResult.Save(Path.Combine(strPhysical, strPrefix));
                        }
                    }
                }
                return Json(new { status = true, fileName = strPrefix });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, fileName = ex.Message });
            }
        }

        public JsonResult InsertNews(HttpPostedFileBase fileData)
        {
            fileData = Request.Files[0];
            Image img = Image.FromStream(fileData.InputStream);
            int intWith = 0;
            int intHeight = 0;
            try
            {
                string strExt = Path.GetExtension(fileData.FileName).ToLower();
                string strName = Guid.NewGuid().ToString() + strExt;
                if (strExt != "gif")
                {
                    intWith = img.Width;
                    intHeight = img.Height;
                }
                string strUrl = Path.Combine(HttpContext.Request.PhysicalApplicationPath + "uploads\\", strName);
                fileData.SaveAs(strUrl);
                return Json(new { status = true, fileName = strName, width = intWith, height = intHeight });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, fileName = ex.Message });
            }
        }


    }
}