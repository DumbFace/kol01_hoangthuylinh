using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KOL_HoangThuyLinh.Areas.admincp.Controllers
{
    [MyAuthorize]
    public class adhomeController : BaseController
    {
        // GET: admincp/adhome
        public ActionResult Index()
        {
            return View();
        }
    }
}