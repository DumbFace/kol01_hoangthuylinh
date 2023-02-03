using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMS1_Core.ViewComponents
{
    public class GetTagsHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int page = 1){
            return View(new myfunc().GetListTagView(4));
        }
    }
}