using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using kol01_hoangthuylinh.Models;
using kol01_hoangthuylinh.Helpers;
using cloudscribe.Pagination.Models;

namespace kol01_hoangthuylinh.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        string url = Constants.urlHost;
        ViewBag.url = url;
        ViewBag.Image = url + "Content/Images/logohome.jpg";
        ViewBag.aboutMe = new myfunc().GetProfile();
        ViewBag.Title = new myfunc().GetConfigWeb().TitleWeb;

        return View();
    }
    public ActionResult GetLatesNews()
    {
        List<tblHome> lst =new myfunc().GetLastNewsHome(8);
        return PartialView(lst);
    }

    public ActionResult GetList(int page = 1, int pagesize = 12)
    {
        PagedResult<tblHome> lst = new myfunc().GetNewsHome(page, pagesize);
        return PartialView(lst);
    }

    public ActionResult GetTagsView()
    {
        List<tblTag> lst = new myfunc().GetListTagView(10);
        return PartialView(lst);
    }

}
