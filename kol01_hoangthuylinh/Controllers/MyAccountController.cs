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
    public class MyAccountController : Controller
    {
        public const string hsSessionName = "TuiKhon_User";
        private IHttpContextAccessor _httpContext;

        public MyAccountController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        // GET: MyAccount
        public ActionResult Index()
        {
            return View();
        }

        [Route("/dang-nhap")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("/dang-nhap")]
        [HttpPost]
        public ActionResult Login(tblUser model)
        {
            model.Password = myCommon.Encrypt(model.Password);
            tblUser tbl = new myfunc().CheckLoginUser(model);
            if (tbl == null)
            {
                return Content("0");
            }
            else
            {
                MyAuthorize.SetLoggedUser(HttpContext, tbl);
                return Content("1");
            }
        }

        public ActionResult Logout()
        {
            MyAuthorize.LogOut(HttpContext);
            return RedirectToAction("Login");
        }

        [Route("/thay-doi-mat-khau")]
        public ActionResult ChangePassword()
        {
            ViewBag.Id = HttpContext.Session.GetObject<tblUser>(myCommon.hsSessionName).Id;
            if (ViewBag.Id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [Route("/access-denied")]
        public IActionResult AccessDenied(){
            return View();
        }
    }
}