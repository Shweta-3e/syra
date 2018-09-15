using System.Web;
using System.Web.Optimization;

namespace Syra.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-2.1.4.min.js",
                "~/syra_assets/d975e8eef86fa13dac350752bb833cac.js",
           "~/Scripts/notifIt.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                               "~/Scripts/angular/angular.min.js",
                               "~/Scripts/angular/angular-route.min.js",
                               "~/Scripts/angular/angular-ui-router.js",
                               "~/Scripts/angular/angular-ui-router.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularapp").Include(
             //Admin module
             "~/Appscript/syraroute.js",
             "~/Appscript/SyraService.js",
             "~/Appscript/Register/RegisterController.js",
              "~/Appscript/ChatBot/ChatBotController.js",
               "~/Appscript/AdminPlan/AdminPlanController.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/css/notifIt.css",
                      "~/Content/site.css"
                      ));
        }
    }
}
