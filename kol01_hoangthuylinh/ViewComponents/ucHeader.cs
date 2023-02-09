using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace kol01_hoangthuylinh.ViewComponents
{
    public class ucHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(){
            return View();
        }
    }
}