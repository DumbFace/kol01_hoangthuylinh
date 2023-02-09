using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace kol01_hoangthuylinh.ViewComponents
{
    public class GetTagsViewViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(){
            return View(new myfunc().GetListTagView(10));
        }
    }
}