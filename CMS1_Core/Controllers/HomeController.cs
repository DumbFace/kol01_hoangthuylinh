using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CMS1_Core.Models;
using CMS1_Core.Helpers;
namespace CMS1_Core.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        myfunc myfunc = new myfunc();
        ViewData["Tags"] = new myfunc().GetListTagView();
        tblConfig tbl = myfunc.GetConfigWeb();
        ViewBag.Title = tbl.TitleWeb;
        ViewBag.Description = tbl.DescriptionWeb;

        string url = Constants.urlHost;
        ViewBag.url = url;
        ViewBag.Image = url + "/Images/logohome.png";
        ViewBag.showHead = 1;
        ViewBag.isHomeFooter = 1;
        return View();
    }

    public IActionResult GetLatesNews()
    {
        return PartialView(new myfunc().GetLastNewsHome(8));
    }

    public IActionResult GetListNews(int page = 1, int pagesize = 10)
    {
        return PartialView(new myfunc().GetNewsHome(page));
    }

    public IActionResult GetTagsView()
    {
        return PartialView(new myfunc().GetListTagView(10));
    }

    public IActionResult GetTagsHeader()
    {
        return PartialView(new myfunc().GetListTagView().Take(4));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
