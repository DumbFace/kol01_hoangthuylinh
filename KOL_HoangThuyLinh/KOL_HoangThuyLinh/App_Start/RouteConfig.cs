using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KOL_HoangThuyLinh
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "ChangePassword",
                url: "thay-doi-mat-khau",
                defaults: new { controller = "MyAccount", action = "ChangePassword" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "MyAccount", action = "Login" }
                );
            routes.MapRoute(
                 name: "tag",
                 url: "tag/{url}-{id}.html",
                 defaults: new { controller = "Tag", action = "Index", url = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            routes.MapRoute(
                    name: "Gallery",
                    url: "bo-suu-tap",
                    defaults: new { controller = "Gallery", action = "Index", url = UrlParameter.Optional, id = UrlParameter.Optional }
                );

            routes.MapRoute(
                    name: "Albums",
                    url: "bo-suu-tap/{url}-{id}",
                    defaults: new { controller = "Gallery", action = "GalleryAlbum", url = UrlParameter.Optional, id = UrlParameter.Optional }
                );
            routes.MapRoute(
                    name: "News",
                    url: "bai-viet",
                    defaults: new { controller = "News", action = "Index", url = UrlParameter.Optional, id = UrlParameter.Optional }
                );
            routes.MapRoute(
                    name: "Video",
                    url: "bai-viet-video",
                    defaults: new { controller = "Video", action = "Index", url = UrlParameter.Optional, id = UrlParameter.Optional }
                );

            routes.MapRoute(
                    name: "Details",
                    url: "{url}-{id}.html",
                    defaults: new { controller = "Read", action = "Index", url = UrlParameter.Optional, id = UrlParameter.Optional });
           
            routes.MapRoute(name: "Search", url: "tim-kiem/q={name}", defaults: new { controller = "Search", action = "Index", name = (string)null });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
