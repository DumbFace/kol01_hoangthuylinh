using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace kol01_hoangthuylinh.Areas.admincp.Controllers
{
    [Area("Admincp")]
    [Route("admincp")]
    public class AdminCPController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}