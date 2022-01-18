using System.Web;
using System.Web.Optimization;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/fullcalandarbundle").Include(
                        "~/Assests/Dashboard/assets/vendor/magnific-popup/magnific-popup.css",
                        "~/Assests/Dashboard/assets/vendor/bootstrap-datepicker/css/datepicker3.css",
                        "~/Assests/Dashboard/assets/vendor/pnotify/pnotify.custom.css",
                        "~/Assests/Dashboard/assets/vendor/fullcalendar/fullcalendar.css",
                        "~/Assests/Dashboard/assets/stylesheets/theme.css",
                        "~/Assests/Dashboard/assets/stylesheets/skins/default.css",
                        "~/Assests/Dashboard/assets/stylesheets/theme-custom.css",
                        "~/Assests/Dashboard/assets/vendor/modernizr/modernizr.js"));

            bundles.Add(new ScriptBundle("~/bundles/Js_fullcalandarbundle").Include(
                "~/Assests/Dashboard/assets/vendor/jquery/jquery.js",
                "~/Assests/Dashboard/assets/vendor/jquery-browser-mobile/jquery.browser.mobile.js",
                "~/Assests/Dashboard/assets/vendor/nanoscroller/nanoscroller.js",
                "~/Assests/Dashboard/assets/vendor/magnific-popup/magnific-popup.js",
                "~/Assests/Dashboard/assets/vendor/bootstrap-datepicker/js/bootstrap-datepicker.js",
                "~/Assests/Dashboard/assets/vendor/jquery-placeholder/jquery.placeholder.js",
                "~/Assests/Dashboard/assets/vendor/pnotify/pnotify.custom.js",
                "~/Assests/Dashboard/assets/vendor/jquery-ui/js/jquery-ui-1.10.4.custom.js",
                "~/Assests/Dashboard/assets/vendor/fullcalendar/lib/moment.min.js",
                "~/Assests/Dashboard/assets/vendor/fullcalendar/fullcalendar.js",
                "~/Assests/Dashboard/assets/javascripts/theme.js",
                "~/Assests/Dashboard/assets/javascripts/theme.custom.js",
                "~/Assests/Dashboard/assets/javascripts/theme.init.js",

                "~/Assests/Dashboard/assets/javascripts/ui-elements/examples.modals.js",
                "~/Assests/Dashboard/assets/javascripts/forms/examples.advanced.form.js"));          


            bundles.Add(new StyleBundle("~/bundles/manualPosting").Include(
                        "~/Assests/Dashboard/assets/vendor/bootstrap-timepicker/css/bootstrap-timepicker.css",
                        "~/Assests/abc/datatables.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/ManualPostinJs").Include(
                "~/Assests/abc/datatables.min.js",
                "~/Assests/abc/dataTables.checkboxes.min.js",               
                "~/Assests/Dashboard/assets/vendor/nanoscroller/nanoscroller.js",
                "~/Assests/Dashboard/assets/vendor/bootstrap-timepicker/js/bootstrap-timepicker.js",
                "~/Assests/Dashboard/assets/vendor/bootstrap-datepicker/js/bootstrap-datepicker.js",
                "~/Assests/Dashboard/assets/javascripts/theme.js",
                "~/Assests/Dashboard/assets/javascripts/theme.init.js"));

            // for check box switch
            bundles.Add(new ScriptBundle("~/bundles/switch").Include(                       
                       "~/Assests/Dashboard/assets/vendor/nanoscroller/nanoscroller.js",
                       "~/Assests/Dashboard/assets/vendor/ios7-switch/ios7-switch.js",
                       "~/Assests/Dashboard/assets/javascripts/theme.js",
                       "~/Assests/Dashboard/assets/javascripts/theme.init.js"
                       ));
            // for check box switch
            bundles.Add(new StyleBundle("~/bundles/switch_Css").Include(
                       "~/Assests/Dashboard/assets/stylesheets/theme.css",
                       "~/Assests/Dashboard/assets/stylesheets/skins/default.css"                       
                       ));
            // for check box switch
            bundles.Add(new ScriptBundle("~/bundles/Track_js").Include(
                       "~/Assests/Dashboard/assets/vendor/nanoscroller/nanoscroller.js",
                       "~/Assests/Dashboard/assets/vendor/jquery-appear/jquery.appear.js",
                       "~/Assests/Dashboard/assets/javascripts/theme.js",
                       "~/Assests/Dashboard/assets/javascripts/theme.init.js"
                       ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
