using System.Web.Optimization;

namespace IDIMAdmin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery",
                "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.1.1.min.js")
                .Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymig").Include(
                        "~/Scripts/jquery-migrate-3.0.0.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/respond.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/font-awesome/css/font-awesome.min.css",
                "~/Scripts/Plugins/pace/pace-theme-flash.css",
                "~/Scripts/Plugins/bootstrap/css/bootstrap.min.css",
                "~/Scripts/Plugins/bootstrap/css/bootstrap-theme.min.css",
                "~/Scripts/Plugins/jquery-loading/src/loading.css",
                "~/Content/animate.min.css",
                "~/Scripts/Plugins/perfect-scrollbar/perfect-scrollbar.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                "~/Scripts/jquery.easing.min.js",
                "~/Scripts/Plugins/bootstrap/js/bootstrap.min.js",
                "~/Scripts/Plugins/daterangepicker/js/moment.min.js",
                "~/Scripts/Plugins/pace/pace.min.js",
                "~/Scripts/Plugins/jquery-loading/src/loading.js",
                "~/Scripts/Plugins/perfect-scrollbar/perfect-scrollbar.min.js",
                "~/Scripts/Plugins/viewport/viewportchecker.js"
            ));

            bundles.Add(new StyleBundle("~/Content/admin").Include(
                "~/Content/style.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Scripts/Theme/scripts.js",
                "~/Scripts/default.js"
            ));

            bundles.Add(new StyleBundle("~/content/login").Include(
                "~/Content/login.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Scripts/default.js"
            ));

            #region dashboard
            bundles.Add(new StyleBundle("~/Content/dashboard").Include(
                "~/Scripts/Plugins/morris-chart/css/morris.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                "~/Scripts/Plugins/sparkline-chart/jquery.sparkline.min.js",
                "~/Scripts/Plugins/easypiechart/jquery.easypiechart.min.js",
                "~/Scripts/Plugins/morris-chart/js/raphael-min.js",
                "~/Scripts/Plugins/morris-chart/js/morris.min.js",
                "~/Scripts/Theme/chart-sparkline.js",
                "~/Scripts/Theme/admin.js"
            ));
            #endregion

            #region form
            bundles.Add(new StyleBundle("~/Content/form").Include(
                "~/Scripts/Plugins/icheck/skins/all.css",
                "~/Scripts/Plugins/select2/select2.css",
                "~/Scripts/Plugins/bootstrap-touchspin/jquery.bootstrap-touchspin.css",
                "~/Scripts/Plugins/datepicker/css/datepicker.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                "~/Scripts/Plugins/icheck/icheck.min.js",
                "~/Scripts/Plugins/select2/select2.min.js",
                "~/Scripts/Plugins/bootstrap-touchspin/jquery.bootstrap-touchspin.js",
                "~/Scripts/Plugins/datepicker/js/datepicker.js"
            ));
            #endregion

            #region vector map
            bundles.Add(new ScriptBundle("~/bundles/jvectormap").Include(
                        "~/Scripts/Plugins/jvectormap/jquery-jvectormap-2.0.1.min.js",
                        "~/Scripts/Plugins/jvectormap/jquery-jvectormap-world-mill-en.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/vectormap").Include(
                        "~/Scripts/Plugins/jvectormap/jquery-jvectormap-2.0.1.css"
                        ));
            #endregion

            #region datatable
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                        "~/Scripts/Plugins/datatables/js/jquery.dataTables.min.js",
                        "~/Scripts/Plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js",
                        "~/Scripts/Plugins/datatables/extensions/Responsive/js/dataTables.responsive.min.js",
                        "~/Scripts/Plugins/datatables/extensions/Responsive/bootstrap/3/dataTables.bootstrap.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                        "~/Scripts/Plugins/datatables/css/jquery.dataTables.min.css",
                        "~/Scripts/Plugins/datatables/extensions/TableTools/css/dataTables.tableTools.min.css",
                        "~/Scripts/Plugins/datatables/extensions/Responsive/css/dataTables.responsive.css",
                        "~/Scripts/Plugins/datatables/extensions/Responsive/bootstrap/3/dataTables.bootstrap.css"
                        ));
            #endregion

            BundleTable.EnableOptimizations = false;
        }
    }
}
