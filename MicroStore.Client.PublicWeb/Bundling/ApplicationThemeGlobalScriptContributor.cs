using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
namespace MicroStore.Client.PublicWeb.Bundling
{

    public class ApplicationThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/summernote/summernote.min.js",
                "/libs/admin-lte/js/adminlte.min.js",
                "/libs/chart.js/dist/chart.umd.js",
                "/libs/chartjs-adapter-luxon/dist/chartjs-adapter-luxon.umd.min.js",
                "/js/abp.config.js"
            });
        }
    }
}
