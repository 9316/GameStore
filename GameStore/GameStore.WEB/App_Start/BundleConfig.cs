using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace GameStore.WEB.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css"));

            bundles.Add(new ScriptBundle("~/content/ErrorStyles").Include(
                      "~/Content/ErrorStyles.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-mobile").Include(
                      "~/Scripts/jquery-1.9.1.min.js",
                      "~/Scripts/jquery.mobile-1.3.2.min.js"));

            bundles.Add(new StyleBundle("~/Content/jquery-mobile").Include(
                      "~/Content/jquery.mobile-1.3.2.min.css"));

            bundles.Add(new StyleBundle("~/bundles/Validation").Include(
                      "~/Scripts/jquery.validate.js",
                      "~/Scripts/jquery.validate.unobtrusive.js"));

        }
    }
}