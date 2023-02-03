using System.Web;
using System.Web.Optimization;

namespace KOL_HoangThuyLinh
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-3.4.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/adbootstrap").Include(
                "~/Scripts/bootstrap.bundle.min.js",
                 "~/Scripts/bootbox.min.js",
                 "~/Scripts/custom.min.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/jspost").Include(
                "~/Scripts/jsContent.min.js",
                "~/Scripts/bootstrap_tagsinput.min.js"));

            bundles.Add(new StyleBundle("~/Content/cssupload").Include(
               "~/Content/jquery.fileupload.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jsupload").Include(
               "~/Scripts/jquery.ui.widget.min.js",
               "~/Scripts/jquery.fileupload.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/adcss").Include(
                 "~/Content/bootstrap.min.css",
                   "~/Content/font-awesome.min.css",
                 "~/Content/custom.min.css"
                 ));
             
            bundles.Add(new Bundle("~/bundles/jshome").Include(
                "~/Scripts/jquery-3.4.1.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/bootstrap.bundle.min.js",
                "~/Scripts/swiper-8.3.2.min.js"
             ));


            bundles.Add(new StyleBundle("~/Content/csshome").Include(
               "~/Content/bootstrap.min.css",
                "~/Content/font-awesome.min.css",
                "~/Content/swiper.min.css",
                //"~/Content/site.min.css",
                "~/Content/site.css",
                //"~/Content/responsive.min.css"
                "~/Content/responsive.css"

            ));

            //BundleTable.EnableOptimizations = true;
        }
    }
}
