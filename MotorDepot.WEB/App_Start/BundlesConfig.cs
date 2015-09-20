using System.Web.Optimization;

namespace MotorDepot.WEB
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/reset.css",
                "~/Content/css/main.css",
                "~/Content/css/font-awesome.css",
                "~/Content/css/ripple-effect.css",
                "~/Content/css/forms.css"));

            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.easing.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/formScripts").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.autogrow-textarea.js",
                "~/Scripts/jquery.ripple-effect.js",
                "~/Scripts/forms.js"));

            bundles.Add(new ScriptBundle("~/Scripts/layout").Include(
                "~/Scripts/layout.js"));

            bundles.Add(new ScriptBundle("~/Scripts/vehicleScripts").Include(
                "~/Scripts/vehicles.js"));

            bundles.Add(new ScriptBundle("~/Scripts/voyageScripts").Include(
                "~/Scripts/voyages.js"));

            bundles.Add(new ScriptBundle("~/Scripts/userScripts").Include(
                "~/Scripts/users.js"));
        }
    }
}