using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;

namespace kol01_hoangthuylinh.ViewComponents
{
    public class GetListTagViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<tblTag> tags){
            return View(tags);
        }
    }
}