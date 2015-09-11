using System.Web.Optimization;

namespace MotorDepot.WEB
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/main.css",
                "~/Content/css/font-awesome.css"));

        }
    }
}