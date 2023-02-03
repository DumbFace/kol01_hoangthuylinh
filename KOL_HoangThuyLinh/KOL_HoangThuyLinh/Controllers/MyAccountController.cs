﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Controllers
{
    public class MyAccountController : Controller
    {
        public const string hsSessionName = "TuiKhon_User";

        // GET: MyAccount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

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

        [MyAuthorize]
        public ActionResult ChangePassword()
        {
            ViewBag.UserName = ((tblUser)Session[hsSessionName]).UserName;
            ViewBag.Id = ((tblUser)Session[hsSessionName]).Id;
            if (ViewBag.UserName == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public string CheckCurrentPass(string oldPass)
        {
            tblUser userLogin = MyAuthorize.GetLoggedUser(HttpContext);
            userLogin.Password = myCommon.Encrypt(oldPass);
            tblUser tbl = new myfunc().CheckLoginUser(userLogin);
            return tbl == null ? "0" : "1";
        }
    }
}