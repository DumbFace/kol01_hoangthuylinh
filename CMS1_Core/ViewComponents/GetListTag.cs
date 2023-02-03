using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS1_Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMS1_Core.ViewComponents
{
    public class GetListTagViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<tblTag> tags){
            return View(tags);
        }
    }
}