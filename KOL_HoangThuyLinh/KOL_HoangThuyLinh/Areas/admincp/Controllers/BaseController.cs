using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh.Areas.admincp.Controllers
{
    public class BaseController : Controller
    {
        public bool IsLogged
        {
            get
            {
                return (MyAuthorize.GetLoggedUser(HttpContext) != null);
            }
        }

        public tblUser GetLoggedUser
        {
            get
            {
                return MyAuthorize.GetLoggedUser(HttpContext);
            }
        }
    }
}