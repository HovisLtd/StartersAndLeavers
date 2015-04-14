using System.Web.Optimization;

namespace Hovis.Web.Base
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
           "~/Content/bootstrap.css",
           "~/Content/vendor/bootstrap-select/bootstrap-select.css",
           "~/Content/vendor/dropzone/dropzone.css",
           "~/Content/vendor/slider/slider.css",
           "~/Content/vendor/bootstrap-datepicker/datepicker.css",
           "~/Content/vendor/timepicker/jquery.timepicker.css",
           "~/Content/vendor/offline/theme.css",
           "~/Content/vendor/pace/theme.css",
           "~/Content/css/font-awesome.css",
           "~/Content/css/animate.css",
           "~/Content/css/skins/palette.1.css",
           "~/Content/css/fonts/style.1.css",
           "~/Content/css/main.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/vendor/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Content/vendor/bootstrap-select/bootstrap-select.js",
                      "~/Content/vendor/bootstrap-datepicker/bootstrap-datepicker.js",
                      "~/Scripts/respond.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}