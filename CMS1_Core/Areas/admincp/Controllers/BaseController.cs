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