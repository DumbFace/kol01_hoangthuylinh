using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CMS1_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMS1_Core.Areas.admincp.Controllers
{
    [Area("Admincp")]
    [MyAuthorize(Roles = "Admin")]
    public class ManageUsersController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListUser(int page = 1, string role = "%", string name = "", int pagesize = 10)
        {
            return PartialView(new myfunc().GetListUser(page, role, name, pagesize));
        }

        public ActionResult AddUser()
        {
            return View();
        }

        public ActionResult IsExistUserName(string str)
        {
            return Content(new myfunc().IsExistUserName(str) ? "1" : "ok");
        }

        public ActionResult InsUser(tblUser tbl)
        {
            tbl.Password = myCommon.Encrypt(tbl.Password);
            new myfunc().InsertUser(tbl);
            return Content("ok");
        }

        public ActionResult ResetPass(int Id)
        {
            ViewBag.IdUser = Id;
            return PartialView();
        }

        public ActionResult UpdatePass(int Id, string str)
        {
            new myfunc().ResetPass(Id, myCommon.Encrypt(str));
            return Content("ok");
        }

        public ActionResult GetPassword()
        {
            return Content(myCommon.RandomPass());
        }

        public ActionResult ShowRoleUser(int Id)
        {
            return PartialView(new myfunc().GetUserbyId(Id));
        }

        public ActionResult UpdateRoleUser(int Id, int IdRole)
        {
            new myfunc().UpdateRole(Id, IdRole);
            return Content("ok");
        }

        public ActionResult RemoveUser(int Id)
        {
            new myfunc().DeleteUser(Id);
            return Content("ok");
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