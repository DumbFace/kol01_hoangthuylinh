using System.Web;
using System.Web.Mvc;

namespace KOL_HoangThuyLinh
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
