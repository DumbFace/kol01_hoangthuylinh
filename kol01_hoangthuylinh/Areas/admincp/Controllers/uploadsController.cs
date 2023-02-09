using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace kol01_hoangthuylinh.Areas.admincp.Controllers
{
    [Area("Admincp")]
    public class uploadsController : Controller
    {

        private readonly IWebHostEnvironment env;

        public uploadsController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpPost]
        // GET: admincp/uploads
        public async Task<JsonResult> SaveImageGalleryAsync(IFormFile file)
        {

            try
            {
                var uploadDirecotroy = "uploads/";
                var uploadPath = Path.Combine(env.WebRootPath, uploadDirecotroy);
                string strName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, strName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Json(new
                {
                    status = true,
                    fileName = strName
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, fileName = ex.Message });
            }

        }

        [HttpPost]
        // GET: admincp/uploads
        public async Task<JsonResult> SaveImageGallerysAsync(List<IFormFile> files)
        {
            List<tblGalleryDT> lst = new List<tblGalleryDT>();
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].Length > 0)
                {
                    var uploadDirecotroy = "uploads/";
                    var uploadPath = Path.Combine(env.WebRootPath, uploadDirecotroy);
                    string strName = Guid.NewGuid().ToString() + Path.GetExtension(files[i].FileName);
                    string filePath = Path.Combine(uploadPath, strName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await files[i].CopyToAsync(stream);
                    }

                    tblGalleryDT tbl = new tblGalleryDT() { Avatar = strName };
                    lst.Add(tbl);
                }
            }
            return Json(new { status = true, tbl = lst });
        }

        // [HttpPost]
        // // GET: admincp/uploads
        // public JsonResult SaveImageEditor(HttpPostedFileBase fileData)
        // {
        //     fileData = Request.Files[0];
        //     try
        //     {
        //         string strName = Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName);
        //         string strPhysical = HttpContext.Request.PhysicalApplicationPath + "uploads\\Content\\";
        //         string strUrl = Path.Combine(strPhysical, strName);
        //         fileData.SaveAs(strUrl);
        //         return Json(new { status = true, fileName = strName });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(new { status = false, fileName = ex.Message });
        //     }
        // }

        // [HttpPost]
        // // GET: admincp/uploads
        // public JsonResult SaveImageAvatarThumb(HttpPostedFileBase fileData, int positionImage)
        // {
        //     //Kiểm tra kích thước
        //     fileData = Request.Files[0];
        //     Image img = Image.FromStream(fileData.InputStream);
        //     int intHeight = img.Height;
        //     int intWidth = img.Width;
        //     switch (positionImage)
        //     {
        //         case 2:
        //             if (intWidth != 800 || intHeight != 400)
        //                 return Json(new { status = "ErrorDimension", message = "Bạn vui lòng chọn kích thước 800x400" });
        //             break;
        //         case 3:
        //             if (intWidth != 400 || intHeight != 800)
        //                 return Json(new { status = "ErrorDimension", message = "Bạn vui lòng chọn kích thước 400x800" });
        //             break;
        //         case 4:
        //             if (intWidth != 800 || intHeight != 800)
        //                 return Json(new { status = "ErrorDimension", message = "Bạn vui lòng chọn kích thước 800x800" });
        //             break;
        //     }
        //     try
        //     {
        //         string strName = Guid.NewGuid().ToString() + Path.GetExtension(fileData.FileName);
        //         string strPhysical = HttpContext.Request.PhysicalApplicationPath + "uploads\\";
        //         string strUrl = Path.Combine(strPhysical, strName);
        //         fileData.SaveAs(strUrl);
        //         return Json(new { status = true, fileName = strName });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(new { status = false, fileName = ex.Message });
        //     }
        // }

        // public JsonResult SaveThumb(string strName, int intCropX, int intCropY, int intWidth, int intHeight)
        // {
        //     try
        //     {
        //         string strPhysical = HttpContext.Request.PhysicalApplicationPath + "uploads\\";
        //         string strPrefix = "1x1_" + strName;
        //         using (Image image = Image.FromFile(Path.Combine(strPhysical, strName)))
        //         {
        //             using (Bitmap bmp = new Bitmap(intWidth, intHeight))
        //             {
        //                 bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
        //                 using (Graphics Graphic = Graphics.FromImage(bmp))
        //                 {
        //                     Graphic.SmoothingMode = SmoothingMode.AntiAlias;
        //                     Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                     Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //                     Graphic.DrawImage(image, new Rectangle(0, 0, intWidth, intHeight), intCropX, intCropY, intWidth, intHeight, GraphicsUnit.Pixel);
        //                     MemoryStream ms = new MemoryStream();
        //                     bmp.Save(ms, image.RawFormat);
        //                     Image imgResult = Image.FromStream(new MemoryStream(ms.ToArray()));
        //                     imgResult.Save(Path.Combine(strPhysical, strPrefix));
        //                 }
        //             }
        //         }
        //         return Json(new { status = true, fileName = strPrefix });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(new { status = false, fileName = ex.Message });
        //     }
        // }

        // public JsonResult InsertNews(HttpPostedFileBase fileData)
        // {
        //     fileData = Request.Files[0];
        //     Image img = Image.FromStream(fileData.InputStream);
        //     int intWith = 0;
        //     int intHeight = 0;
        //     try
        //     {
        //         string strExt = Path.GetExtension(fileData.FileName).ToLower();
        //         string strName = Guid.NewGuid().ToString() + strExt;
        //         if (strExt != "gif")
        //         {
        //             intWith = img.Width;
        //             intHeight = img.Height;
        //         }
        //         string strUrl = Path.Combine(HttpContext.Request.PhysicalApplicationPath + "uploads\\", strName);
        //         fileData.SaveAs(strUrl);
        //         return Json(new { status = true, fileName = strName, width = intWith, height = intHeight });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(new { status = false, fileName = ex.Message });
        //     }
        // }


    }
}
