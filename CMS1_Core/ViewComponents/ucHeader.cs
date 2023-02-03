using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMS1_Core.ViewComponents
{
    public class ucHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(){
            return View();
        }
    }
}